using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.ObjectStateManagerTests
{
    [TestClass]
    public class TheSetStateMethod : MsTestMoqTestBase<TestableObjectStateManager>
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

            TestRunner
                .DoCustomSetup(() =>
                {
                    var hash = DataGenerator.GenerateString();

                    testClass = ObjectCreator.CreateNew<TestClassWithPrimaryKey>();

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
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(testClass);

                    Asserter.AssertEquality(1, ItemUnderTest.ObjectStates.Count);
                    Assert.IsTrue(ItemUnderTest.ObjectStates.ContainsKey(key));
                    Asserter.AssertEquality(key, ItemUnderTest.ObjectStates.Keys.First());
                    Asserter.AssertEquality(expected, ItemUnderTest.ObjectStates[key], 
                        new[] {"ObjectType", "ChildObjectStates"});
                    Asserter.AssertEquality(expected.ObjectType.ToString(), ItemUnderTest.ObjectStates[key].ObjectType.ToString());
                    Assert.IsTrue(!ItemUnderTest.ObjectStates[key].ChildObjectStates.Any());
                });
        }

        [TestMethod]
        public void WillSetObjectStatesForComplexObject()
        {
            InitialObjectState expected = null;
            TestClassWithPrimaryKeyWithEnumerable testClass = null;
            var key = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var hash = DataGenerator.GenerateString();
                    var childHash = DataGenerator.GenerateString();
                    var child = ObjectCreator.CreateNew<TestClassWithPrimaryKey>();

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
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(testClass);

                    Asserter.AssertEquality(1, ItemUnderTest.ObjectStates.Count);
                    Assert.IsTrue(ItemUnderTest.ObjectStates.ContainsKey(key));
                    Asserter.AssertEquality(key, ItemUnderTest.ObjectStates.Keys.First());
                    Asserter.AssertEquality(expected, ItemUnderTest.ObjectStates[key],
                        new[] { "ObjectType", "ChildObjectStates" });
                    Asserter.AssertEquality(expected.ObjectType.ToString(), ItemUnderTest.ObjectStates[key].ObjectType.ToString());
                    Asserter.AssertEquality(expected.ChildObjectStates, ItemUnderTest.ObjectStates[key].ChildObjectStates,
                        new[] { "ObjectType", "ChildObjectStates" });
                });
        }

        [TestMethod]
        public void WillUpdateObjectState()
        {
            var expectedHash = String.Empty;
            var id = 0;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var hash = DataGenerator.GenerateString();
                    var objectStates = new ConcurrentDictionary<string, InitialObjectState>();

                    expectedHash = DataGenerator.GenerateString();
                    id = DataGenerator.GenerateInteger();

                    objectStates.TryAdd(id.ToString(), new InitialObjectState
                    {
                        HashCode = hash,
                        Id = id.ToString(),
                        ObjectType = typeof (TestClassWithPrimaryKey)
                    });

                    ItemUnderTest.ObjectStates = objectStates;

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(It.IsAny<object>()))
                        .Returns(expectedHash);
                })
                .ExecuteTest(() =>
                {
                    var item = new TestClassWithPrimaryKey
                    {
                        Id = id
                    };

                    ItemUnderTest.SetState(item);

                    Assert.IsTrue(ItemUnderTest.ObjectStates.Count == 1);
                    Assert.IsTrue(ItemUnderTest.ObjectStates.ContainsKey(id.ToString()));
                    var state = ItemUnderTest.ObjectStates[id.ToString()];

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

            TestRunner
                .DoCustomSetup(() =>
                {
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

                    var objectStates = new ConcurrentDictionary<string, InitialObjectState>();

                    objectStates.TryAdd(child.Id.ToString(),
                        new InitialObjectState
                        {
                            Id = child.Id.ToString(),
                            HashCode = currentChildHash,
                            ObjectType = typeof (TestClassWithPrimaryKey)
                        });

                    objectStates.TryAdd(parent1.TestId.ToString(),
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

                    objectStates.TryAdd(parent2.TestId.ToString(),
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

                    ItemUnderTest.ObjectStates = objectStates;

                    Mocks.Get<Mock<IHashHelper>>()
                      .Setup(x => x.GenerateHash(It.IsAny<object>()))
                      .Returns(expectedHash);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(child);

                    Asserter.AssertEquality(3, ItemUnderTest.ObjectStates.Count);
                    Asserter.AssertEquality(expectedHash, ItemUnderTest.ObjectStates[child.Id.ToString()].HashCode);
                    Asserter.AssertEquality(expectedHash, ItemUnderTest.ObjectStates[parent1.TestId.ToString()].ChildObjectStates.First().HashCode);
                    Asserter.AssertEquality(expectedHash, ItemUnderTest.ObjectStates[parent2.TestId.ToString()].ChildObjectStates.First().HashCode);
                });
        }

        [TestMethod]
        public void WillCleanUpOrphans()
        {
            TestClassWithPrimaryKeyWithEnumerable parent = null;
            TestClassWithPrimaryKey expectedChild = null;

            TestRunner
                .DoCustomSetup(() =>
                {
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

                    var objectStates = new ConcurrentDictionary<string, InitialObjectState>();

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

                    objectStates.TryAdd("1", objectState);

                    ItemUnderTest.ObjectStates = objectStates;
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.SetState(parent);

                    var childStates = ItemUnderTest.ObjectStates.First().Value.ChildObjectStates; 

                    Asserter.AssertEquality(1, childStates.Count);
                    Asserter.AssertEquality(true, childStates.Where(c => c.Id == "2").IsNullOrEmpty());
                    Asserter.AssertEquality(true, childStates.Where(c => c.Id == "1").IsNotNullOrEmpty());
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