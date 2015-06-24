using System;
using System.Data;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    //public abstract class ExportableDatabaseRepositoryBase<TInterface, TModel> : PagingDatabaseRepositoryBase<TInterface, TModel>
    //    where TModel : class, TInterface, new()
    //{
    //    protected ExportableDatabaseRepositoryBase(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IDataReaderBuilder builder, IModelDataConverter modelDataConverter,
    //        IBuilderStrategyFactory builderDelegateStrategyFactory, IBuilder<IDataReader, PagingModel> pagingModelBuilder)
    //        : base(databaseName, databaseFactory, mapper, queryBuilder, builder, modelDataConverter, 
    //        builderDelegateStrategyFactory, pagingModelBuilder)
    //    {
    //    }

    //    public virtual IDatatableObjectMapping ToDataTable(Expression filterExpression = null,
    //        PagingInfo pagingInfo = null)
    //    {
    //        var castExpression = filterExpression as Expression<Func<TModel, bool>>;

    //        var models = pagingInfo.IsNull() 
    //            ? Get(castExpression)
    //            : base.GetCriteriaByPage(castExpression, pagingInfo).Items
    //                .SafeSelect(s => (TModel)s);

    //        return ModelDataConverter.ConvertToDataTable(models);
    //    }

    //    public virtual void TransformData(Action<TInterface> transformAction, 
    //        Expression filterExpression = null,
    //        PagingInfo pagingInfo = null, bool includeParameters = true)
    //    {
    //        Guard.EnsureIsNotNull("transformAction", transformAction);

    //        var castExpression = filterExpression as Expression<Func<TModel, bool>>; 

    //        if (pagingInfo.IsNull())
    //        {
    //            ExecuteTransform(QueryBuilder.BuildSelectQuery(castExpression, true, includeParameters),
    //                r => Builder.Build<TModel>(r), transformAction);
    //        }
    //        else
    //        {
    //            ExecuteTransform(QueryBuilder.BuildPagedQuery(pagingInfo, castExpression, includeParameters:includeParameters), 
    //                r => Builder.Build<TModel>(r), transformAction);
    //        }
    //    }
    //}
}
