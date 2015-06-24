using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public class PagingRepositoryHelper<TInterface, TModel> : ReadDatabaseRepositoryBase<TInterface, TModel>
        where TModel : class, TInterface, new()
    {
        protected readonly IBuilder<IDataReader, PagingModel> PagingModelBuilder;
        protected readonly IDataReaderBuilder Builder;

        protected PagingRepositoryHelper(string databaseName, IDatabaseFactory databaseFactory, 
            IMapper mapper, IQueryBuilder queryBuilder, IDataReaderBuilder builder, 
            IBuilderStrategyFactory builderStrategyFactory,
            IBuilder<IDataReader, PagingModel> pagingModelBuilder) 
            : base(databaseName, databaseFactory, mapper, queryBuilder,  
                builderStrategyFactory)
        {
            PagingModelBuilder = Guard.EnsureIsNotNull("pagingModelBuilder", pagingModelBuilder);
            Builder = Guard.EnsureIsNotNull("builder", builder);
        }

        public virtual PagingModel GetCriteriaByPage(Expression filterExpression = null, PagingInfo pagingInfo = null, 
            bool includeParameters = true)
        {
            if (pagingInfo.IsNull())
            {
                pagingInfo = new PagingInfo
                {
                    PageNumber = 1,
                    RowsPerPage = 50,
                    SortColumn = "Id",
                    SortOrder = "DESC"
                };
            }

            PagingModel pagingModel = null;
            var models = new List<object>();
            var castExpression = filterExpression as Expression<Func<TModel, bool>>;

            var queryInfo = QueryBuilder.BuildPagedQuery(pagingInfo, castExpression,
                includeParameters: includeParameters);

            Database.CreateCommandText(queryInfo.Query, QueryType.Text)
                .WithParameters(queryInfo.Parameters)
                .ExecuteMultiple(r =>
                {
                    if (pagingModel.IsNull())
                    {
                        pagingModel = PagingModelBuilder.Build<PagingModel>(r);
                    }

                    models.Add(Builder.Build<TModel>(r));

                    return pagingModel;
                });

            if (pagingModel.IsNotNull())
            {
                pagingModel.Items = models;
                pagingModel.CurrentPage = pagingInfo.PageNumber;
                pagingModel.RowsPerPage = pagingInfo.RowsPerPage;
            }

            return pagingModel;
        }
    }
}