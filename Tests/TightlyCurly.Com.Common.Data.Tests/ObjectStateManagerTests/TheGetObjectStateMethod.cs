using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Tests.Common.MsTest;
using TightlyCurly.Com.Common.Data.Attributes;

namespace TightlyCurly.Com.Common.Data.Tests.ObjectStateManagerTests
{
    [TestClass]
    public class TheGetObjectStateMethod : MsTestMoqTestBase<ObjectStateManager>
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
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.GetObjectState(null))
                    .AndVerifyHasParameter("value");
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
                            () => ItemUnderTest.GetObjectState(new object()))
                        .AndVerifyMessageContains("Cannot set state.  Object has no primary key defined");
                });
        }

        [TestMethod]
        public void WillInvokeHashHelper()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.GetObjectState(ObjectCreator.CreateNew<ParentTestClass>());

                    Mocks.Get<Mock<IHashHelper>>()
                        .Verify(x => x.GenerateHash(It.IsAny<ParentTestClass>()), 
                            Times.Once);
                });
        }

        [TestMethod]
        public void WillReturnObjectStateNewIfObjectIsNotPresentInDictionaryAndHasNoChildren()
        {
            ChildTestClass value = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    value = ObjectCreator.CreateNew<ChildTestClass>();
                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(new Dictionary<string, InitialObjectState>());
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(value);

                    Assert.IsNotNull(actual);
                    Asserter.AssertEquality(typeof(ChildTestClass).ToString(), actual.ObjectType.ToString());
                    Asserter.AssertEquality(ObjectState.New, actual.ObjectState);
                    Assert.IsTrue(actual.PropertyName.IsNullOrEmpty());
                    Assert.IsTrue(actual.ChildInfos.IsNullOrEmpty());
                });
        }

        [TestMethod]
        public void WillReturnChildStates()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var notTracked = ObjectCreator.CreateNew<ChildTestClass>();
                    var attached = ObjectCreator.CreateNew<ChildTestClass>();
                    var detached = ObjectCreator.CreateNew<ChildTestClass>();
                    var attachedToParentAsChildNoParentExists = ObjectCreator.CreateNew<ChildTestClass>();
               
                    parent = ObjectCreator.CreateNew<ParentTestClass>(new Dictionary<string, object>
                    {
                        {"Children", new[] {notTracked, attached, detached, attachedToParentAsChildNoParentExists}}
                    });

                    var objectStates = new Dictionary<string, InitialObjectState>();

                    var parentState = new InitialObjectState
                    {
                        HashCode = DataGenerator.GenerateString(),
                        Id = parent.ParentId.ToString(),
                        ObjectType = typeof (ParentTestClass),
                        ChildObjectStates = new []
                        {
                            new InitialObjectState
                            {
                                HashCode = DataGenerator.GenerateString(),
                                Id = attached.ChildId.ToString(),
                                ObjectType = typeof(ChildTestClass),
                                PropertyName = "Children",
                                ParentId = parent.ParentId.ToString()
                            },
                            new InitialObjectState
                            {
                                HashCode = DataGenerator.GenerateString(),
                                Id = attachedToParentAsChildNoParentExists.ChildId.ToString(),
                                ObjectType = typeof(ChildTestClass),
                                PropertyName = "Children",
                                ParentId = parent.ParentId.ToString()
                            }
                        }
                    };

                    objectStates.Add(parent.ParentId.ToString(), parentState);

                    objectStates.Add(attached.ChildId.ToString(), new InitialObjectState
                    {
                        HashCode = DataGenerator.GenerateString(),
                        Id = attached.ChildId.ToString(),
                        ObjectType = typeof(ChildTestClass),
                    });

                    objectStates.Add(detached.ChildId.ToString(), new InitialObjectState
                    {
                        HashCode = DataGenerator.GenerateString(),
                        Id = detached.ChildId.ToString(),
                        ObjectType = typeof(ChildTestClass)
                    });

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsNotNull(actual);
                    Asserter.AssertEquality(typeof(ParentTestClass).ToString(), actual.ObjectType.ToString());
                    Assert.IsFalse(actual.ChildInfos.IsNullOrEmpty());
                    Assert.AreEqual(4, actual.ChildInfos.Count());

                    var childInfo = actual.ChildInfos.First();

                    Assert.IsNotNull(childInfo);
                    Asserter.AssertEquality("Children", childInfo.PropertyName);
                    Asserter.AssertEquality(ChildState.NotTracked, childInfo.ChildState);

                    childInfo = actual.ChildInfos.Take(2).Last();

                    Assert.IsNotNull(childInfo);
                    Asserter.AssertEquality("Children", childInfo.PropertyName);
                    Asserter.AssertEquality(ChildState.Attached, childInfo.ChildState);

                    childInfo = actual.ChildInfos.Take(3).Last();

                    Assert.IsNotNull(childInfo);
                    Asserter.AssertEquality("Children", childInfo.PropertyName);
                    Asserter.AssertEquality(ChildState.Detached, childInfo.ChildState);

                    childInfo = actual.ChildInfos.Last();

                    Assert.IsNotNull(childInfo);
                    Asserter.AssertEquality("Children", childInfo.PropertyName);
                    Asserter.AssertEquality(ChildState.AttachedToParentAsChildNoParentExists, childInfo.ChildState);
                });
        }

        [TestMethod]
        public void WillReturnNoChangeIfObjectHasntChanged()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parent = ObjectCreator.CreateNew<ParentTestClass>();
                    var hashCode = DataGenerator.GenerateString();

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(hashCode);

                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = false,
                            HashCode = hashCode
                        });

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsNotNull(actual);
                    Asserter.AssertEquality(ObjectState.NoChange, actual.ObjectState);
                });
        }

        [TestMethod]
        public void WillReturnUpdatedIfHashCodesDontMatch()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parent = ObjectCreator.CreateNew<ParentTestClass>();
                    var hashCode = DataGenerator.GenerateString();

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(hashCode);

                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = false,
                            HashCode = DataGenerator.GenerateString()
                        });

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsNotNull(actual);
                    Asserter.AssertEquality(ObjectState.Updated, actual.ObjectState);
                });
        }

        [TestMethod]
        public void WillReturnDeletedIfStateIsDeleted()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parent = ObjectCreator.CreateNew<ParentTestClass>();
                    var hashCode = DataGenerator.GenerateString();

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(hashCode);

                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = true,
                            HashCode = DataGenerator.GenerateString()
                        });

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsNotNull(actual);
                    Asserter.AssertEquality(ObjectState.Deleted, actual.ObjectState);
                });
        }

        [TestMethod]
        public void WillAssignNewToChildObjectAndChildObjectIsNotParent()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var child = ObjectCreator.CreateNew<ChildTestClass>();

                    parent = ObjectCreator.CreateNew<ParentTestClass>(new Dictionary<string, object>
                    {
                        {"Children", new[] {child}}
                    });

                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof (ParentTestClass),
                            IsDeleted = false,
                            HashCode = DataGenerator.GenerateString()
                        });

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(DataGenerator.GenerateString());
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsFalse(actual.ChildInfos.IsNullOrEmpty());
                    Asserter.AssertEquality(1, actual.ChildInfos.Count);
                    Asserter.AssertEquality(ObjectState.New, actual.ChildInfos.First().ObjectState);
                });
        }

        [TestMethod]
        public void WillAssignNoChangeToChildObjectAndChildObjectIsParent()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var child = ObjectCreator.CreateNew<ChildTestClass>();

                    parent = ObjectCreator.CreateNew<ParentTestClass>(new Dictionary<string, object>
                    {
                        {"Children", new[] {child}}
                    });

                    var childHash = DataGenerator.GenerateString();
                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = false,
                            HashCode = DataGenerator.GenerateString(),
                            ChildObjectStates = new []
                            {
                                new InitialObjectState
                                {
                                    Id = child.ChildId.ToString(),
                                    HashCode = childHash,
                                    ObjectType = typeof(ChildTestClass),
                                    ParentId = parent.ParentId.ToString(),
                                    PropertyName = "Children"
                                }
                            }
                        });

                    objectStates.Add(child.ChildId.ToString(),
                        new InitialObjectState
                        {
                            Id = child.ChildId.ToString(),
                            HashCode = childHash,
                            ObjectType = typeof (ChildTestClass),
                        });

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(child))
                        .Returns(childHash);

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsFalse(actual.ChildInfos.IsNullOrEmpty());
                    Asserter.AssertEquality(1, actual.ChildInfos.Count);
                    Asserter.AssertEquality(ObjectState.NoChange, actual.ChildInfos.First().ObjectState);
                });
        }

        [TestMethod]
        public void WillAssignUpdatedToChildObjectAndChildObjectIsParent()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var child = ObjectCreator.CreateNew<ChildTestClass>();

                    parent = ObjectCreator.CreateNew<ParentTestClass>(new Dictionary<string, object>
                    {
                        {"Children", new[] {child}}
                    });

                    var childHash = DataGenerator.GenerateString();
                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = false,
                            HashCode = DataGenerator.GenerateString(),
                            ChildObjectStates = new[]
                            {
                                new InitialObjectState
                                {
                                    Id = child.ChildId.ToString(),
                                    HashCode = childHash,
                                    ObjectType = typeof(ChildTestClass),
                                    ParentId = parent.ParentId.ToString(),
                                    PropertyName = "Children"
                                }
                            }
                        });

                    objectStates.Add(child.ChildId.ToString(),
                        new InitialObjectState
                        {
                            Id = child.ChildId.ToString(),
                            HashCode = childHash,
                            ObjectType = typeof(ChildTestClass),
                        });

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(child))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsFalse(actual.ChildInfos.IsNullOrEmpty());
                    Asserter.AssertEquality(1, actual.ChildInfos.Count);
                    Asserter.AssertEquality(ObjectState.Updated, actual.ChildInfos.First().ObjectState);
                });
        }

        [TestMethod]
        public void WillAssignDeletedToChildObjectAndChildObjectIsParentAndDeleted()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var child = ObjectCreator.CreateNew<ChildTestClass>();

                    parent = ObjectCreator.CreateNew<ParentTestClass>(new Dictionary<string, object>
                    {
                        {"Children", new[] {child}}
                    });

                    var childHash = DataGenerator.GenerateString();
                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = false,
                            HashCode = DataGenerator.GenerateString(),
                            ChildObjectStates = new[]
                            {
                                new InitialObjectState
                                {
                                    Id = child.ChildId.ToString(),
                                    HashCode = childHash,
                                    ObjectType = typeof(ChildTestClass),
                                    ParentId = parent.ParentId.ToString(),
                                    PropertyName = "Children"
                                }
                            }
                        });

                    objectStates.Add(child.ChildId.ToString(),
                        new InitialObjectState
                        {
                            Id = child.ChildId.ToString(),
                            IsDeleted = true,
                            HashCode = childHash,
                            ObjectType = typeof(ChildTestClass),
                        });

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(child))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsFalse(actual.ChildInfos.IsNullOrEmpty());
                    Asserter.AssertEquality(1, actual.ChildInfos.Count);
                    Asserter.AssertEquality(ObjectState.Deleted, actual.ChildInfos.First().ObjectState);
                });
        }

        [TestMethod]
        public void WillAssignDeletedToChildObjectIfChildIsDeletedAndChildObjectIsParentAndNotDeleted()
        {
            ParentTestClass parent = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var child = ObjectCreator.CreateNew<ChildTestClass>();

                    parent = ObjectCreator.CreateNew<ParentTestClass>(new Dictionary<string, object>
                    {
                        {"Children", new[] {child}}
                    });

                    var childHash = DataGenerator.GenerateString();
                    var objectStates = new Dictionary<string, InitialObjectState>();

                    objectStates.Add(parent.ParentId.ToString(),
                        new InitialObjectState
                        {
                            Id = parent.ParentId.ToString(),
                            ObjectType = typeof(ParentTestClass),
                            IsDeleted = false,
                            HashCode = DataGenerator.GenerateString(),
                            ChildObjectStates = new[]
                            {
                                new InitialObjectState
                                {
                                    Id = child.ChildId.ToString(),
                                    HashCode = childHash,
                                    IsDeleted = true,
                                    ObjectType = typeof(ChildTestClass),
                                    ParentId = parent.ParentId.ToString(),
                                    PropertyName = "Children"
                                }
                            }
                        });

                    objectStates.Add(child.ChildId.ToString(),
                        new InitialObjectState
                        {
                            Id = child.ChildId.ToString(),
                            IsDeleted = false,
                            HashCode = childHash,
                            ObjectType = typeof(ChildTestClass),
                        });

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(parent))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IHashHelper>>()
                        .Setup(x => x.GenerateHash(child))
                        .Returns(DataGenerator.GenerateString());

                    Mocks.Get<Mock<IStateStore>>()
                        .Setup(x => x.GetStateStore())
                        .Returns(objectStates);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetObjectState(parent);

                    Assert.IsFalse(actual.ChildInfos.IsNullOrEmpty());
                    Asserter.AssertEquality(1, actual.ChildInfos.Count);
                    Asserter.AssertEquality(ObjectState.Deleted, actual.ChildInfos.First().ObjectState);
                });
        }

        public class ParentTestClass
        {
            public ParentTestClass()
            {
                Children = new List<ChildTestClass>();
            }

            [FieldMetadata("ParentId", isPrimaryKey: true)]
            public int ParentId { get; set; }

            [Join(JoinType.Left, typeof(ChildTestClass), "Id", "Id")]
            public IEnumerable<ChildTestClass> Children { get; set; } 
        }

        public class ChildTestClass
        {
            [FieldMetadata("ChildId", isPrimaryKey: true)]
            public int ChildId { get; set; }
        }
    }
}
