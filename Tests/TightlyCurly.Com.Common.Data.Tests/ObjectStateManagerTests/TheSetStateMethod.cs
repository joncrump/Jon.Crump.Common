using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.ObjectStateManagerTests
{
    [TestClass]
    public class TheSetStateMethod : MsTestMoqTestBase<ObjectStateManager>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IHashHelper>>()
                .Setup(x => x.GenerateHash(It.IsAny<object>()))
                .Returns(DataGenerator.GenerateString());
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfValueIsNull()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<ArgumentNullException>(
                            () => ItemUnderTest.SetState(null))
                        .AndVerifyHasParameter("value");
                });
        }

        [TestMethod]
        public void WillInvokeHashHelper()
        {
            TestRunner
                .DoCustomSetup(() =>
                {
                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(new Dictionary<string, InitialObjectState>());
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(ObjectCreator.CreateNew<TestClassWithPrimaryKey>());

                    Mocks.Get<Mock<IHashHelper>>()
                        .Verify(x => x.GenerateHash(It.IsAny<object>()), 
                            Times.Once);
                });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfPrimaryKeyIsMissing()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<InvalidOperationException>(
                            () => ItemUnderTest.SetState(new object()))
                        .AndVerifyMessageContains("Cannot set state.  Object has no primary key defined");
                });
        }

        [TestMethod]
        public void WillSetObjectStateOnSimpleObject()
        {
            InitialObjectState expected = null;
            TestClassWithPrimaryKey testClass = null;
            var key = String.Empty;
            IDictionary<string, InitialObjectState> states = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var hash = DataGenerator.GenerateString();

                    testClass = ObjectCreator.CreateNew<TestClassWithPrimaryKey>();
                    states = new Dictionary<string, InitialObjectState>();

                    var property = typeof(TestClassWithPrimaryKey)
                        .GetProperties()
                        .First(p => p.Name == "Id");

                    key = property.GetValue(testClass).ToString();

                    expected = new InitialObjectState
                    {
                        Id = key,
                        ObjectType = typeof(TestClassWithPrimaryKey),
                        HashCode = hash
                    };

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(testClass))
                        .Returns(hash);

                    var stateStore = Mocks.Get<Mock<IStateStore>>();

                    stateStore
                        .Setup(x => x.GetStateStore())
                        .Returns(states);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(testClass);
                    
                    Asserter.AssertEquality(1, states.Count);
                    Assert.IsTrue(states.ContainsKey(key));
                    Asserter.AssertEquality(key, states.Keys.First());
                    Asserter.AssertEquality(expected, states[key], 
                        new[] {"ObjectType", "ChildObjectStates"});
                    Asserter.AssertEquality(expected.ObjectType.ToString(), states[key].ObjectType.ToString());
                    Assert.IsTrue(!states[key].ChildObjectStates.Any());
                });
        }

        [TestMethod]
        public void WillSetObjectStatesForComplexObject()
        {
            InitialObjectState expected = null;
            TestClassWithPrimaryKeyWithEnumerable testClass = null;
            var key = String.Empty;
            IDictionary<string, InitialObjectState> states = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var hash = DataGenerator.GenerateString();
                    var childHash = DataGenerator.GenerateString();
                    var child = ObjectCreator.CreateNew<TestClassWithPrimaryKey>();
                    states = new Dictionary<string, InitialObjectState>();

                    testClass = ObjectCreator.CreateNew<TestClassWithPrimaryKeyWithEnumerable>(
                        new Dictionary<string, object>
                        {
                            {"TestClasses", new [] {child}}   
                        });

                    var property = typeof(TestClassWithPrimaryKeyWithEnumerable)
                        .GetProperties()
                        .First(p => p.Name == "TestId");

                    key = property.GetValue(testClass).ToString();

                    var childProperty = typeof(TestClassWithPrimaryKey)
                        .GetProperties()
                        .First(p => p.Name == "Id");

                    var childKey = childProperty.GetValue(child).ToString();

                    expected = new InitialObjectState
                    {
                        Id = key,
                        ObjectType = typeof(TestClassWithPrimaryKeyWithEnumerable),
                        HashCode = hash,
                        ChildObjectStates = new []
                        {
                            new InitialObjectState
                            {
                                Id = childKey,
                                HashCode = childHash, 
                                ObjectType = typeof(TestClass),
                                PropertyName = "TestClasses",
                                ParentId = key
                            }
                        }
                    };

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(testClass))
                        .Returns(hash);

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(child))
                        .Returns(childHash);

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(states);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(testClass);

                    Asserter.AssertEquality(1, states.Count);
                    Assert.IsTrue(states.ContainsKey(key));
                    Asserter.AssertEquality(key, states.Keys.First());
                    Asserter.AssertEquality(expected, states[key],
                        new[] { "ObjectType", "ChildObjectStates" });
                    Asserter.AssertEquality(expected.ObjectType.ToString(), states[key].ObjectType.ToString());
                    Asserter.AssertEquality(expected.ChildObjectStates, states[key].ChildObjectStates,
                        new[] { "ObjectType", "ChildObjectStates" });
                });
        }

        [TestMethod]
        public void WillUpdateObjectState()
        {
            var expectedHash = String.Empty;
            var id = 0;
            IDictionary<string, InitialObjectState> states = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var hash = DataGenerator.GenerateString();

                    states = new Dictionary<string, InitialObjectState>();
                    expectedHash = DataGenerator.GenerateString();
                    id = DataGenerator.GenerateInteger();

                    states.Add(id.ToString(), new InitialObjectState
                    {
                        HashCode = hash,
                        Id = id.ToString(),
                        ObjectType = typeof (TestClassWithPrimaryKey)
                    });

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(It.IsAny<object>()))
                        .Returns(expectedHash);

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(states);
                })
                .ExecuteTest(() =>
                {
                    var item = new TestClassWithPrimaryKey
                    {
                        Id = id
                    };

                    ItemUnderTest.SetState(item);

                    Assert.IsTrue(states.Count == 1);
                    Assert.IsTrue(states.ContainsKey(id.ToString()));
                    var state = states[id.ToString()];

                    Asserter.AssertEquality(expectedHash, state.HashCode);
                });
        }

        [TestMethod]
        public void WillUpdateAllStatesForAnInstanceOfObject()
        {
            TestClassWithPrimaryKey child = null;
            TestClassWithPrimaryKeyWithEnumerable parent1 = null;
            TestClassWithPrimaryKeyWithEnumerable parent2 = null;
            string expectedHash = null;
            string currentChildHash = null;
            IDictionary<string, InitialObjectState> states = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    states = new Dictionary<string, InitialObjectState>();
                    currentChildHash = DataGenerator.GenerateString();
                    expectedHash = DataGenerator.GenerateString();
                    child = ObjectCreator.CreateNew<TestClassWithPrimaryKey>();
                    parent1 = ObjectCreator.CreateNew<TestClassWithPrimaryKeyWithEnumerable>(
                        new Dictionary<string, object>
                        {
                            {"TestId", 1},
                            {"TestClasses", new[] {child}}
                        });
                    parent2 = ObjectCreator.CreateNew<TestClassWithPrimaryKeyWithEnumerable>(
                        new Dictionary<string, object>
                        {
                            {"TestId", 2},
                            {"TestClasses", new[] {child}}
                        });

                    states.Add(child.Id.ToString(),
                        new InitialObjectState
                        {
                            Id = child.Id.ToString(),
                            HashCode = currentChildHash,
                            ObjectType = typeof (TestClassWithPrimaryKey)
                        });

                    states.Add(parent1.TestId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent1.TestId.ToString(),
                            HashCode = DataGenerator.GenerateString(),
                            ObjectType = typeof (TestClassWithPrimaryKeyWithEnumerable),
                            ChildObjectStates = new List<InitialObjectState>
                            {
                                new InitialObjectState
                                {
                                    Id = child.Id.ToString(),
                                    HashCode = currentChildHash,
                                    ObjectType = typeof (TestClassWithPrimaryKey)
                                }
                            }
                        });

                    states.Add(parent2.TestId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent2.TestId.ToString(),
                            HashCode = DataGenerator.GenerateString(),
                            ObjectType = typeof (TestClassWithPrimaryKeyWithEnumerable),
                            ChildObjectStates = new List<InitialObjectState>
                            {
                                new InitialObjectState
                                {
                                    Id = child.Id.ToString(),
                                    HashCode = currentChildHash,
                                    ObjectType = typeof (TestClassWithPrimaryKey)
                                }
                            }
                        });

                    Mocks.Get<Mock<IHashHelper>>()
                      .Setup(x => x.GenerateHash(It.IsAny<object>()))
                      .Returns(expectedHash);

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(states);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(child);

                    Asserter.AssertEquality(3, states.Count);
                    Asserter.AssertEquality(expectedHash, states[child.Id.ToString()].HashCode);
                    Asserter.AssertEquality(expectedHash, states[parent1.TestId.ToString()].ChildObjectStates.First().HashCode);
                    Asserter.AssertEquality(expectedHash, states[parent2.TestId.ToString()].ChildObjectStates.First().HashCode);
                });
        }

        [TestMethod]
        public void WillCleanUpOrphans()
        {
            TestClassWithPrimaryKeyWithEnumerable parent = null;
            TestClassWithPrimaryKey expectedChild = null;
            IDictionary<string, InitialObjectState> states = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    states = new Dictionary<string, InitialObjectState>();

                    expectedChild = ObjectCreator.CreateNew<TestClassWithPrimaryKey>(
                        new Dictionary<string, object>
                        {
                            {"Id", 1}
                        });

                    parent = ObjectCreator.CreateNew<TestClassWithPrimaryKeyWithEnumerable>(
                        new Dictionary<string, object>
                        {
                            {"TestId", 1},
                            {"TestClasses", new[] {expectedChild}}
                        });

                    var objectState = new InitialObjectState
                    {
                        Id = "1",
                        HashCode = DataGenerator.GenerateString(),
                        ObjectType = typeof (TestClassWithPrimaryKeyWithEnumerable),
                        ChildObjectStates = new List<InitialObjectState>
                        {
                            new InitialObjectState
                            {
                                Id = "1",
                                HashCode = DataGenerator.GenerateString(),
                                ObjectType = typeof(TestClassWithPrimaryKey),
                                PropertyName = "TestClasses"
                            },
                            new InitialObjectState
                            {
                                Id = "2",
                                HashCode = DataGenerator.GenerateString(),
                                ObjectType = typeof(TestClassWithPrimaryKey),
                                PropertyName = "TestClasses"
                            }
                        }
                    };

                    states.Add("1", objectState);

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(states);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(parent);

                    var childStates = states.First().Value.ChildObjectStates; 

                    Asserter.AssertEquality(1, childStates.Count);
                    Asserter.AssertEquality(true, childStates.FirstOrDefault(c => c.Id == "2").IsDeleted);
                    Asserter.AssertEquality(true, childStates.FirstOrDefault(c => c.Id == "1").IsDeleted);
                });
        }

        public class TestClassWithPrimaryKey
        {
            [FieldMetadata("Id", isPrimaryKey: true)]
            public int Id { get; set; }
        }

        public class TestClassWithPrimaryKeyWithEnumerable
        {
            [FieldMetadata("TestId", isPrimaryKey: true)]
            public int TestId { get; set; }

            [Join(JoinType.Left, typeof(TestClassWithPrimaryKey), "Id", "Id")]
            public IEnumerable<TestClassWithPrimaryKey> TestClasses { get; set; }
        }
    }
}