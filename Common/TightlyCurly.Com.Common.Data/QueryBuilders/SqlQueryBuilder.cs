using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders
{
    public class SqlQueryBuilder : IQueryBuilder
    {
        private readonly IQueryBuilderStrategyFactory _builderStrategyFactory;

        public SqlQueryBuilder(IQueryBuilderStrategyFactory queryBuilderStrategyFactory)
        {
            _builderStrategyFactory = Guard.EnsureIsNotNull("queryBuilderStrategyFactory", queryBuilderStrategyFactory);
        }

        public QueryInfo BuildSelectQuery<TValue>(Expression<Func<TValue, bool>> predicate = null,
            bool canDirtyRead = false, bool includeParameters = true, IEnumerable<string> desiredFields = null, 
            string tableName = null, BuildMode buildMode = BuildMode.Single) where TValue : class
        {
            var queryInfo = buildMode == BuildMode.Single 
                ? BuildSingleSelectQuery(predicate, canDirtyRead, includeParameters, desiredFields, tableName)
                : BuildJoinedSelectQuery(predicate, canDirtyRead, includeParameters, desiredFields, tableName);

            return queryInfo;
        }
        
        public QueryInfo BuildInsertQuery<TValue>(TValue model, bool returnNewId = true, bool ignoreIdentity = true,
            IEnumerable<string> desiredFields = null, string tableName = null) where TValue : class
        {
            Guard.EnsureIsNotNull("model", model);

            dynamic parameters = new ExpandoObject();

            parameters.Model = model;
            parameters.ReturnNewId = returnNewId;
            parameters.IgnoreIdentity = ignoreIdentity;
            parameters.DesiredFields = desiredFields;
            parameters.TableName = tableName;

            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.Insert);
            var info = strategy.BuildQuery<TValue>(parameters);

            return info;
        }

        public QueryInfo BuildUpdateQuery<TValue>(TValue model, Expression<Func<TValue, bool>> predicate, 
            IEnumerable<string> desiredFields = null, string tableName = null) where TValue : class
        {
            Guard.EnsureIsNotNull("model", model);
            Guard.EnsureIsNotNull("predicate", predicate);

            dynamic parameters = new ExpandoObject();

            parameters.Model = model;
            parameters.Predicate = predicate;
            parameters.DesiredFields = desiredFields;
            parameters.TableName = tableName;

            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.Update);
            var info = strategy.BuildQuery<TValue>(parameters);

            return info;
        }

        public QueryInfo BuildDeleteQuery<TValue>(Expression<Func<TValue, bool>> predicate, string tableName = null) where TValue : class
        {
            Guard.EnsureIsNotNull("predicate", predicate);

            dynamic parameters = new ExpandoObject();

            parameters.TableName = tableName;
            parameters.Predicate = predicate;

            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.Delete);

            var queryInfo = strategy.BuildQuery<TValue>(parameters);

            return queryInfo;
        }
        
        public QueryInfo BuildCountQuery<TValue>() where TValue : class
        {
            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.Count);
            var queryInfo = strategy.BuildQuery<TValue>();

            return queryInfo;
        }

        public QueryInfo BuildPagedQuery<TValue>(PagingInfo pagingInfo, Expression<Func<TValue, bool>> predicate = null, bool canDirtyRead = false,
            bool includeParameters = true, IEnumerable<string> desiredFields = null, string tableName = null) where TValue : class
        {
            if (pagingInfo.IsNull())
            {
                return BuildSelectQuery(predicate, canDirtyRead, includeParameters, desiredFields, tableName);
            }

            dynamic parameters = new ExpandoObject();

            parameters.PagingInfo = pagingInfo;
            parameters.DesiredFields = desiredFields;
            parameters.Predicate = predicate;
            parameters.CanDirtyRead = canDirtyRead;
            parameters.IncludeParameters = includeParameters;

            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.PagedSingle);
            var info = strategy.BuildQuery<TValue>(parameters);

            return info;
        }

        public string BuildSelectQueryFor<TValue>(IEnumerable<string> desiredFields = null) where TValue : class
        {
            return BuildSelectQuery<TValue>(desiredFields: desiredFields).Query.Replace(";", String.Empty);
        }

        public QueryInfo BuildInQueryFor<TParent, TChild>(string whereField, string selectChildField, Expression<Func<TChild, bool>> childPredicate)
            where TParent : class
            where TChild : class
        {
            Guard.EnsureIsNotNullOrEmpty("whereField", whereField);
            Guard.EnsureIsNotNullOrEmpty("selectChildField", selectChildField);

            var outerQuery = BuildSelectQueryFor<TParent>();
            var innerQuery = BuildSelectQuery(childPredicate, desiredFields: new[] { selectChildField });
            var inQuery = "{0} WHERE {1} IN ({2});".FormatString(outerQuery, whereField, innerQuery.Query);

            innerQuery.Query = inQuery;

            return innerQuery;
        }

        private QueryInfo BuildJoinedSelectQuery<TValue>(Expression<Func<TValue, bool>> predicate, bool canDirtyRead, bool includeParameters, IEnumerable<string> desiredFields, string tableName)
            where TValue : class
        {
            dynamic parameters = new ExpandoObject();

            parameters.Predicate = predicate;
            parameters.CanDirtyRead = canDirtyRead;
            parameters.IncludeParameters = includeParameters;
            parameters.DesiredFields = desiredFields;
            parameters.TableName = tableName;

            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.SelectJoinTable);
            var info = strategy.BuildQuery<TValue>(parameters);

            return info;
        }

        private QueryInfo BuildSingleSelectQuery<TValue>(Expression<Func<TValue, bool>> predicate, bool canDirtyRead, bool includeParameters,
            IEnumerable<string> desiredFields, string tableName) where TValue : class
        {
            dynamic parameters = new ExpandoObject();

            parameters.Predicate = predicate;
            parameters.CanDirtyRead = canDirtyRead;
            parameters.IncludeParameters = includeParameters;
            parameters.DesiredFields = desiredFields;
            parameters.TableName = tableName;

            var strategy = _builderStrategyFactory.GetBuilderStrategy(QueryKind.SelectSingleTable);
            var info = strategy.BuildQuery<TValue>(parameters);

            return info;
        }
    }
}
