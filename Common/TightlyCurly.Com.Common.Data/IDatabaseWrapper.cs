using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;

namespace TightlyCurly.Com.Common.Data
{
    public interface IDatabaseWrapper
    {
        IDatabaseWrapper WithParameter(DbParameter parameter);
        IDatabaseWrapper WithParameter<T>(string name, object value, ParameterDirection direction = ParameterDirection.Input);
        IDatabaseWrapper WithParameters(IEnumerable<IDbDataParameter> parameters);
        IDatabaseWrapper CreateCommandText(string commandText, QueryType queryType = QueryType.StoredProcedure);
        int ExecuteNonQuery();
        IEnumerable<T> ExecuteMultiple<T>(Func<IDataReader, T> readerDelegate);
        void ExecuteMultiple(Action<IDataReader> readerDelegate);
        T ExecuteSingle<T>(Func<IDataReader, T> readerDelegate);
        void ExecuteSingle(Action<IDataReader> readerDelegate);
        void ExecuteBulk(DataTable dataTable, IDictionary<string, string> columnMappings);
        void ExecuteTransform<T>(Func<IDataReader, T> readerDelegate, Action<T> transformAction);
        IEnumerable<T> ExecuteMultiple<T>(Func<IDataReader, IDataReaderBuilder, IEnumerable<TableObjectMapping>, IEnumerable<T>> readerDelegate,
            IDataReaderBuilder dataReaderBuilder, IEnumerable<TableObjectMapping> tableObjectMappings);
        T ExecuteSingle<T>(Func<IDataReader, IDataReaderBuilder, IEnumerable<TableObjectMapping>, T> readerDelegate,
            IDataReaderBuilder dataReaderBuilder, IEnumerable<TableObjectMapping> tableObjectMappings);
        T ExecuteSingle<T>(IBuilderStrategy builderStrategy, IEnumerable<TableObjectMapping> tableObjectMappings)
            where T : class, new();
        IEnumerable<T> ExecuteMultiple<T>(IBuilderStrategy builderStrategy, IEnumerable<TableObjectMapping> tableObjectMappings)
            where T : class, new();
    }
}
