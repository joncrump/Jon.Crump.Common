using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.QueryBuilders.Strategies.TSql.PagedQueryBuilderStrategyTests
{
    [TestClass]
    public class TheBuildQueryMethod : MsTestMoqTestBase<PagedQueryBuilderStrategy>
    {
        [TestMethod]
        public void WillBuildQuery()
        {
            string expectedQuery = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expectedQuery =
                        "SET @sortColumn = LOWER(@sortColumn); SET @sortOrder = LOWER(@sortOrder); WITH SortedItems AS ( SELECT ClientId, FirstName, MiddleName, LastName, ROW_NUMBER() OVER ( ORDER BY CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END ASC  ) AS RowNumber, COUNT(ClientId) OVER() AS TotalRecords FROM dbo.Clients (NOLOCK) WHERE [FirstName] LIKE ('Jo%') ) SELECT ClientId, FirstName, MiddleName, LastName, [TotalRecords], ([TotalRecords] + @rowsPerPage - 1) / @rowsPerPage AS NumberOfPages FROM SortedItems WHERE SortedItems.RowNumber BETWEEN ((@pageNumber - 1) * @rowsPerPage) + 1 AND @rowsPerPage * @pageNumber  AND [FirstName] LIKE ('Jo%') ORDER BY CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'firstname' THEN FirstName END ASC, CASE WHEN @sortOrder <> 'desc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END DESC, CASE WHEN @sortOrder <> 'asc' THEN NULL WHEN @sortColumn = 'lastname' THEN LastName END ASC ;";

                    Mocks.Get<Mock<IFieldHelper>>()
                        .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(),
                            It.IsAny<string>(), It.IsAny<ClientDataModel>(), It.IsAny<bool>(), 
                            It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(new TableObjectMapping
                        {
                            TableName = "dbo.Clients",
                            FieldMappings =  new Dictionary<string, FieldParameterMapping>
                                {
                                    {"Id", new FieldParameterMapping("ClientId", "@clientId", SqlDbType.UniqueIdentifier, isIdentity:true)},
                                    {"FirstName", new FieldParameterMapping("FirstName", "@firstName", SqlDbType.NVarChar)},
                                    {"MiddleName", new FieldParameterMapping("MiddleName", "@middleName", SqlDbType.NVarChar)},
                                    {"LastName", new FieldParameterMapping("LastName", "@lastName", SqlDbType.NVarChar)}
                                }
                        });

                    Mocks.Get<Mock<IPredicateBuilder>>()
                        .Setup(x => x.BuildContainer(It.IsAny<Expression>(), It.IsAny<Type>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(new QueryContainer("[FirstName] LIKE ('Jo%')"));
                })
                .ExecuteTest(() =>
                {
                    dynamic parameters = new ExpandoObject();
                    Expression<Func<ClientDataModel, bool>> predicate = m => m.FirstName.StartsWith("Jo%");

                    parameters.PagingInfo = new PagingInfo
                    {
                        PageNumber = 2,
                        RowsPerPage = 50,
                        SortColumn = "firstname",
                        SortOrder = "desc"
                    };

                    parameters.Predicate = predicate;
                    parameters.IncludeParameters = true;
                    parameters.CanDirtyRead = true;
                    parameters.DesiredFields = null;

                    QueryInfo query = ItemUnderTest.BuildQuery<ClientDataModel>(parameters);

                    Asserter.AssertEquality(expectedQuery, query.Query);

                    Assert.AreEqual(4, query.Parameters.Count());
                });
        }

        [Table("dbo.Clients")]
        public class ClientDataModel
        {
            [CountMetadata("ClientId", "ClientIdCount")]
            [FieldMetadata("ClientId", SqlDbType.UniqueIdentifier, "@clientId")]
            public Guid Id { get; set; }

            [SortMetadata("firstname", "FirstName")]
            [FieldMetadata("FirstName", SqlDbType.NVarChar, "@firstName")]
            public string FirstName { get; set; }

            [FieldMetadata("MiddleName", SqlDbType.NVarChar, "@middleName", allowDbNull: true)]
            public string MiddleName { get; set; }

            [SortMetadata("lastname", "LastName")]
            [FieldMetadata("LastName", SqlDbType.NVarChar, "@lastName")]
            public string LastName { get; set; }
        }
    }
}
