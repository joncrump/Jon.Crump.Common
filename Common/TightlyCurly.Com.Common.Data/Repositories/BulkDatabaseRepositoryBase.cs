using System.Collections.Generic;
using System.Data;
using System.Linq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    //public abstract class BulkDatabaseRepositoryBase<TInterface, TModel> : WriteDatabaseRepositoryBase<TInterface, TModel>
    //    where TModel : class, TInterface, new()
    //{
    //    //protected BulkDatabaseRepositoryBase(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IDataReaderBuilder builder, IModelDataConverter modelDataConverter,
    //    //    IBuilderStrategyFactory builderDelegateStrategyFactory, IBuilder<IDataReader, PagingModel> pagingModelBuilder)
    //    //    : base(databaseName, databaseFactory, mapper, queryBuilder, builder, modelDataConverter, 
    //    //    builderDelegateStrategyFactory, pagingModelBuilder)
    //    //{
    //    //}

    //    //public virtual void InsertBulk(IEnumerable<TInterface> models)
    //    //{
    //    //    Guard.EnsureIsNotNullOrEmpty("models", models);

    //    //    var mappedValues = models
    //    //        .Select(m => Mapper.Map<TModel>(m))
    //    //        .ToSafeList();

    //    //    var datatableObjectMapping = ModelDataConverter.ConvertToDataTable(mappedValues);

    //    //    Database.ExecuteBulk(datatableObjectMapping.DataTable, datatableObjectMapping.ColumnMappings);
    //    //}
    //}
}
