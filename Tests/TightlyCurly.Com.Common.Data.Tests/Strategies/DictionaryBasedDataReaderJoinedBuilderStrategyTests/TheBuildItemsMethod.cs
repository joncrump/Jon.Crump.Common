using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common;
using TightlyCurly.Com.Tests.Common.Extensions;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.Strategies.DictionaryBasedDataReaderJoinedBuilderStrategyTests
{
    [TestClass]
    public class TheBuildItemsMethod : MsTestMoqDataItemTestBase<DictionaryBasedDataReaderJoinedBuilderStrategy>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfParametersAreNull()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<ArgumentNullException>(
                            () => ItemUnderTest.BuildItems<object>(null, It.IsAny<IDataReader>()))
                        .AndVerifyHasParameter("parameters");
                });
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfDataSourceIsNull()
        {
            dynamic parameters = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parameters = new ExpandoObject();

                    parameters.tableObjectMappings = new List<TableObjectMapping>
                    {
                        new TableObjectMapping()
                    };
                })
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<ArgumentNullException>(
                            () => ItemUnderTest.BuildItems<object>(parameters, null))
                        .AndVerifyHasParameter("dataSource");
                });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfTypeHasNoFieldAttributes()
        {
            dynamic parameters = null;
            MockDataReader reader = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parameters = new ExpandoObject();
                    parameters.TableObjectMappings = CreateEnumerableOfItems<TableObjectMapping>().ToList();
                    reader = new MockDataReader();
                })
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<InvalidOperationException>(
                            () => ItemUnderTest.BuildItems<ClassWithNoFieldAttributes>(parameters, reader))
                        .AndVerifyMessageContains("Type {0} either has no FieldMetaDataAttributes or no Primary Key defined."
                        .FormatString(typeof(ClassWithNoFieldAttributes).ToString()));
                });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfTypeDoesNotHavePrimaryKey()
        {
            dynamic parameters = null;
            MockDataReader reader = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parameters = new ExpandoObject();
                    parameters.TableObjectMappings = CreateEnumerableOfItems<TableObjectMapping>().ToList();
                    reader = new MockDataReader();
                })
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<InvalidOperationException>(
                            () => ItemUnderTest.BuildItems<ClassWithNoPrimaryKey>(parameters, reader))
                        .AndVerifyMessageContains("Type {0} either has no FieldMetaDataAttributes or no Primary Key defined."
                        .FormatString(typeof(ClassWithNoPrimaryKey).ToString()));
                });
        }

        [TestMethod]
        public void WillBuildItems()
        {
            List<Parent> expected = null;
            MockDataReader reader = null;
            dynamic parameters = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = new List<Parent>();
                    parameters = new ExpandoObject();
                    
                    var dataReaderBuilder = Mocks.Get<Mock<IDataReaderBuilder>>();
                    var tableObjectMappings = new List<TableObjectMapping>
                    {
                        new TableObjectMapping
                        {
                            Prefix = "t1"
                        },
                        new TableObjectMapping
                        {
                            Prefix = "t2",
                            InstancePropertyName = "Child1s",
                            ParentKey = "ParentId",
                            ChildKey = "ParentId",
                            JoinType = typeof(Child1)
                        },
                        new TableObjectMapping
                        {
                            Prefix = "t3",
                            InstancePropertyName = "Child2s",
                            ParentKey = "ParentId",
                            ChildKey = "ParentId",
                            JoinType = typeof(Child2)
                        }
                    };

                    parameters.TableObjectMappings = tableObjectMappings;

                    var parent1Id = Guid.NewGuid();
                    var parent2Id = Guid.NewGuid();

                    var parent1 = new Parent
                    {
                        Id = parent1Id,
                        ParentName = "Parent1",
                        Child1s = new List<Child1>
                        {
                            new Child1
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent1Id,
                                Child1Name = "Parent1Child1_1"
                            },
                            new Child1
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent1Id,
                                Child1Name = "Parent1Child1_2"
                            }
                        },
                        Child2s = new List<Child2>
                        {
                            new Child2
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent1Id,
                                Child2Name = "Parent1Child2_1"
                            },
                            new Child2
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent1Id,
                                Child2Name = "Parent1Child2_2"
                            }
                        }
                    };

                    expected.Add(parent1);

                    var parent2 = new Parent
                    {
                        Id = parent2Id,
                        ParentName = "Parent2",
                        Child1s = new List<Child1>
                        {
                            new Child1
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent2Id,
                                Child1Name = "Parent2Child1_1"
                            },
                            new Child1()
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent2Id,
                                Child1Name = "Parent2Child1_2"
                            }
                        },
                        Child2s = new List<Child2>
                        {
                            new Child2
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent2Id,
                                Child2Name = "Parent2Child2_1"
                            },
                            new Child2
                            {
                                Id = Guid.NewGuid(),
                                ParentId = parent2Id,
                                Child2Name = "Parent2Child2_2"
                            }
                        }
                    };

                    expected.Add(parent2);

                    reader = new MockDataReader(new DataContainer(4, new[]
                    {
                        new ColumnInfo("t1_Id", new object[] {parent1.Id, parent1.Id, parent2.Id, parent2.Id}),
                        new ColumnInfo("t1_ParentName", new object[] {parent1.ParentName, parent1.ParentName, parent2.ParentName, parent2.ParentName}),
                        new ColumnInfo("t2_Id", new object[] {parent1.Child1s.First().Id, parent1.Child1s.Last().Id, parent2.Child1s.First().Id, parent2.Child1s.Last().Id}),
                        new ColumnInfo("t2_ParentId", new object[] {parent1.Child1s.First().ParentId, parent1.Child1s.Last().ParentId, parent2.Child1s.First().ParentId, parent2.Child1s.Last().ParentId}),
                        new ColumnInfo("t2_Child1Name", new object[] {parent1.Child1s.First().Child1Name, parent1.Child1s.Last().Child1Name, parent2.Child1s.First().Child1Name, parent2.Child1s.Last().Child1Name}),
                        new ColumnInfo("t3_Id", new object[] {parent1.Child2s.First().Id, parent1.Child2s.Last().Id, parent2.Child2s.First().Id, parent2.Child2s.Last().Id}),
                        new ColumnInfo("t3_ParentId", new object[] {parent1.Child2s.First().ParentId, parent1.Child2s.Last().ParentId, parent2.Child2s.First().ParentId, parent2.Child2s.Last().ParentId}),
                        new ColumnInfo("t3_Child2Name", new object[] {parent1.Child2s.First().Child2Name, parent1.Child2s.Last().Child2Name, parent2.Child2s.First().Child2Name, parent2.Child2s.Last().Child2Name}) 
                    }));

                    var parent1NoChildren = parent1.ShallowCopy();
                    var parent2NoChildren = parent2.ShallowCopy();

                    parent1NoChildren.Child1s = null;
                    parent1NoChildren.Child2s = null;

                    parent2NoChildren.Child1s = null;
                    parent2NoChildren.Child2s = null;

                    dataReaderBuilder
                        .Setup(x => x.Build<Parent>(reader, "t1", false))
                        .ReturnsInOrder(new [] {parent1NoChildren, parent1NoChildren, parent2NoChildren, parent2NoChildren});

                    dataReaderBuilder
                        .Setup(x => x.Build<Child1>(reader, "t2", false))
                        .ReturnsInOrder(new [] {parent1.Child1s.First(), parent1.Child1s.Last(), parent2.Child1s.First(), parent2.Child1s.Last()});

                    dataReaderBuilder
                        .Setup(x => x.Build<Child2>(reader, "t3", false))
                        .ReturnsInOrder(new [] {parent1.Child2s.First(), parent1.Child2s.Last(), parent2.Child2s.First(), parent2.Child2s.Last()});
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildItems<Parent>(parameters, reader);

                    Expression<Action<Parent, Parent>> compareParent =
                       (e, a) => Asserter.AssertEquality(e, a, new[] {"Child1s", "Child2s"}, null, 
                           It.IsAny<bool>());

                    Expression<Action<Child1, Child1>> compareChild1s =
                       (e, a) => Asserter.AssertEquality(e, a, null, null, It.IsAny<bool>());

                    Expression<Action<Child2, Child2>> compareChild2s =
                        (e, a) => Asserter.AssertEquality(e, a, null, null, It.IsAny<bool>());

                    Asserter.AssertEquality(expected, actual, additionalParameters:new Dictionary<string, object>
                    {
                        {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, compareParent}
                    });

                    IEnumerable<Child1> firstExpectedChild1s = ((IEnumerable<Parent>)expected).First().Child1s;
                    IEnumerable<Child1> firstActualChild1s = ((IEnumerable<Parent>)actual).First().Child1s;

                    Asserter.AssertEquality(firstExpectedChild1s, firstActualChild1s,
                        additionalParameters: new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, compareChild1s}
                        });

                    IEnumerable<Child2> firstExpectedChild2s = ((IEnumerable<Parent>)expected).First().Child2s;
                    IEnumerable<Child2> firstActualChild2s = ((IEnumerable<Parent>)actual).First().Child2s;

                    Asserter.AssertEquality(firstExpectedChild2s, firstActualChild2s,
                        additionalParameters: new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, compareChild2s}
                        });

                    IEnumerable<Child1> lastExpectedChild1s = ((IEnumerable<Parent>)expected).Last().Child1s;
                    IEnumerable<Child1> lastActualChild1s = ((IEnumerable<Parent>)actual).Last().Child1s;

                    Asserter.AssertEquality(lastExpectedChild1s, lastActualChild1s,
                        additionalParameters: new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, compareChild1s}
                        });

                    IEnumerable<Child2> lastExpectedChild2s = ((IEnumerable<Parent>)expected).Last().Child2s;
                    IEnumerable<Child2> lastActualChild2s = ((IEnumerable<Parent>)actual).Last().Child2s;

                    Asserter.AssertEquality(lastExpectedChild2s, lastActualChild2s,
                         additionalParameters: new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, compareChild2s}
                        });
                });
        }

        public class Parent
        {
            [FieldMetadata("Id", SqlDbType.UniqueIdentifier, "@id", isPrimaryKey: true)]
            public Guid Id { get; set; }
            public string ParentName { get; set; }
            public IEnumerable<Child1> Child1s { get; set; }
            public IEnumerable<Child2> Child2s { get; set; }

            public Parent ShallowCopy()
            {
                return this.MemberwiseClone() as Parent;
            }

            public Parent()
            {
                Child1s = new List<Child1>();
                Child2s = new List<Child2>();
            }
        }

        public class Child1
        {
            [FieldMetadata("Id", SqlDbType.UniqueIdentifier, "@id", isPrimaryKey: true)]
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
            public string Child1Name { get; set; }
        }

        public class Child2
        {
            [FieldMetadata("Id", SqlDbType.UniqueIdentifier, "@id", isPrimaryKey: true)]
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
            public string Child2Name { get; set; }
        }

        public class ClassWithNoFieldAttributes
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class ClassWithNoPrimaryKey
        {
            [FieldMetadata("Id")]
            public int Id { get; set; }

            [FieldMetadata("Name")]
            public string Name { get; set; }
        }
    }
}