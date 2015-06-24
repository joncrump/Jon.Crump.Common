using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Common.Data.QueryBuilders
{
    public interface IQueryBuilder
    {
        QueryInfo BuildSelectQuery<TValue>(Expression<Func<TValue, bool>> predicate = null, bool canDirtyRead = false,
            bool includeParameters = true, IEnumerable<string> desiredFields = null, string tableName = null,
            BuildMode buildMode = BuildMode.Single)
            where TValue : class; 
        QueryInfo BuildInsertQuery<TValue>(TValue model, bool returnNewId = true, bool ignoreIdentity = true, 
            IEnumerable<string> desiredFields = null, string tableName = null) where TValue : class;
        QueryInfo BuildUpdateQuery<TValue>(TValue model, Expression<Func<TValue, bool>> predicate, 
            IEnumerable<string> desiredFields = null, string tableName = null)
            where TValue : class;
        QueryInfo BuildDeleteQuery<TValue>(Expression<Func<TValue, bool>> predicate, string tableName = null)
            where TValue : class;
        QueryInfo BuildPagedQuery<TValue>(PagingInfo pagingInfo, Expression<Func<TValue, bool>> predicate = null,
            bool canDirtyRead = false, bool includeParameters = true, IEnumerable<string> desiredFields = null, string tableName = null)
            where TValue : class;
        QueryInfo BuildCountQuery<TValue>() where TValue : class;
        string BuildSelectQueryFor<TValue>(IEnumerable<string> desiredFields = null) where TValue : class;
        QueryInfo BuildInQueryFor<TParent, TChild>(string whereField, string selectChildField,
            Expression<Func<TChild, bool>> childPredicate)
            where TParent : class
            where TChild : class;
    }
}
