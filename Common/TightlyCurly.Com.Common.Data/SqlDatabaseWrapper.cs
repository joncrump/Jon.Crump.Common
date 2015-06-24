using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public class SqlDatabaseWrapper : IDatabaseWrapper
    {
        private readonly string _connectionString;
        private QueryType _queryType;
        private string _commandText;
        private List<IDbDataParameter> _parameters;

        public SqlDatabaseWrapper(string connectionString)
        {
            _connectionString = Guard.EnsureIsNotNullOrEmpty("connectionString", connectionString);
            _parameters = new List<IDbDataParameter>();
        }

        public IDatabaseWrapper WithParameter(DbParameter parameter)
        {
            Guard.EnsureIsNotNull("parameter", parameter);

            _parameters.Add(parameter);

            return this;
        }

        public IDatabaseWrapper WithParameter<T>(string name, object value, ParameterDirection direction = ParameterDirection.Input)
        {
            Guard.EnsureIsNotNullOrEmpty("name", name);

            if (value.IsNull() || (value is string && ((string)value).IsNullOrEmpty()))
            {
                value = DBNull.Value;
            }

            _parameters.Add(new SqlParameter
            {
                ParameterName = name,
                Value = value,
                Direction = direction,
                SqlDbType = GetSqlDbType<T>()
            });

            return this;
        }

        public IDatabaseWrapper WithParameters(IEnumerable<IDbDataParameter> parameters)
        {
            Guard.EnsureIsNotNullOrEmpty("parameters", parameters);

            _parameters.AddRange(parameters.Where(p => !p.IsNull()));

            return this;
        }

        public IDatabaseWrapper CreateCommandText(string commandText, QueryType queryType = QueryType.StoredProcedure)
        {
            Guard.EnsureIsNotNullOrEmpty("commandText", commandText);

            _commandText = commandText;
            _queryType = queryType;

            return this;
        }

        public int ExecuteNonQuery()
        {
            try
            {
                int results;

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        results = command.ExecuteNonQuery();
                    }
                }

                return results;
            }
            finally
            {
                ClearParameters();
            }
        }

        public IEnumerable<T> ExecuteMultiple<T>(Func<IDataReader, IDataReaderBuilder, IEnumerable<TableObjectMapping>, IEnumerable<T>> readerDelegate, 
            IDataReaderBuilder dataReaderBuilder, IEnumerable<TableObjectMapping> tableObjectMappings)
        {
            try
            {
                Guard.EnsureIsNotNull("readerDelegate", readerDelegate);
                Guard.EnsureIsNotNull("dataReaderBuilder", dataReaderBuilder);
                Guard.EnsureIsNotNullOrEmpty("tableObjectMappings", tableObjectMappings);

                List<T> values;

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            values = readerDelegate(reader, dataReaderBuilder, tableObjectMappings).ToSafeList();
                        }
                    }
                }

                return values;
            }
            finally
            {
                ClearParameters();
            }
        }

        public IEnumerable<T> ExecuteMultiple<T>(Func<IDataReader, T> readerDelegate)
        {
            try
            {
                Guard.EnsureIsNotNull("readerDelegate", readerDelegate);

                var values = new List<T>();

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                values.Add(readerDelegate(reader));
                            }
                        }
                    }
                }

                return values;
            }
            finally
            {
                ClearParameters();
            }
        }

        public void ExecuteMultiple(Action<IDataReader> readerDelegate)
        {
            try
            {
                Guard.EnsureIsNotNull("readerDelegate", readerDelegate);

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                readerDelegate(reader);
                            }
                        }
                    }
                }
            }
            finally
            {
                ClearParameters();
            }
        }

        public T ExecuteSingle<T>(Func<IDataReader, T> readerDelegate)
        {
            try
            {
                Guard.EnsureIsNotNull("readerDelegate", readerDelegate);

                var value = default(T);

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                value = readerDelegate(reader);
                            }
                        }
                    }
                }

                return value;
            }
            finally
            {
                ClearParameters();
            }
        }

        public void ExecuteSingle(Action<IDataReader> readerDelegate)
        {
            try
            {
                Guard.EnsureIsNotNull("readerDelegate", readerDelegate);

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                readerDelegate(reader);
                            }
                        }
                    }
                }
            }
            finally
            {
                ClearParameters();
            }
        }

        public T ExecuteSingle<T>(Func<IDataReader, IDataReaderBuilder, IEnumerable<TableObjectMapping>, T> readerDelegate,
            IDataReaderBuilder dataReaderBuilder, IEnumerable<TableObjectMapping> tableObjectMappings)
        {
            try
            {
                Guard.EnsureIsNotNull("readerDelegate", readerDelegate);
                Guard.EnsureIsNotNull("dataReaderBuilder", dataReaderBuilder);
                Guard.EnsureIsNotNullOrEmpty("tableObjectMappings", tableObjectMappings);

                var value = default(T);

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            value = readerDelegate(reader, dataReaderBuilder, tableObjectMappings);
                        }
                    }
                }

                return value;
            }
            finally
            {
                ClearParameters();
            }
        }

        public T ExecuteSingle<T>(IBuilderStrategy builderStrategy, IEnumerable<TableObjectMapping> tableObjectMappings)
            where T : class, new()
        {
            Guard.EnsureIsNotNull("builderStrategy", builderStrategy);

            dynamic parameters = new ExpandoObject();
            parameters.TableObjectMappings = tableObjectMappings;

            try
            {
                var value = default(T);

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            value = builderStrategy.BuildItems<T>(parameters, reader);
                        }
                    }
                }

                return value;
            }
            finally
            {
                ClearParameters();
            }
        }

        public IEnumerable<T> ExecuteMultiple<T>(IBuilderStrategy builderStrategy, IEnumerable<TableObjectMapping> tableObjectMappings)
            where T : class, new()
        {
            Guard.EnsureIsNotNull("builderStrategy", builderStrategy);

            dynamic parameters = new ExpandoObject();
            parameters.TableObjectMappings = tableObjectMappings;

            try
            {
                var values = new List<T>();

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var value = builderStrategy.BuildItems<T>(parameters, reader);

                                values.Add(value);
                            }
                        }
                    }
                }

                return values;
            }
            finally
            {
                ClearParameters();
            }
        }

        public void ExecuteBulk(DataTable dataTable, IDictionary<string, string> columnMappings)
        {
            Guard.EnsureIsNotNull("dataTable", dataTable);
            Guard.EnsureIsNotNullOrEmpty("columnMappings", columnMappings);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var copy = new SqlBulkCopy(connection))
                {
// ReSharper disable once AccessToDisposedClosure
                    columnMappings.SafeForEach(m => copy.ColumnMappings.Add(m.Key, m.Value));

                    copy.DestinationTableName = dataTable.TableName;
                    copy.WriteToServer(dataTable);
                }
            }
        }

        public void ExecuteTransform<T>(Func<IDataReader, T> readerDelegate, Action<T> transformAction)
        {
            try
            {
                Guard.EnsureIsNotNull("transformAction", transformAction);

                VerifyCommandText();

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = GetCommand(_commandText, _queryType, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var value = readerDelegate(reader);

                                transformAction(value);
                            }
                        }
                    }
                }
            }
            finally
            {
                ClearParameters();
            }
        }

        private DbCommand GetCommand(string commandText, QueryType queryType, SqlConnection connection)
        {
            var command = new SqlCommand(commandText);

            switch (queryType)
            {
                case QueryType.StoredProcedure:
                    command.CommandType = CommandType.StoredProcedure;
                    break;
                case QueryType.Text:
                    command.CommandType = CommandType.Text;
                    break;
            }

            command.Connection = connection;
            command.CommandTimeout = connection.ConnectionTimeout;

            command.Parameters.Clear();

            foreach (var parameter in _parameters)
            {
                command.Parameters.Add((SqlParameter)parameter);
            }

            return command;
        }

        private SqlDbType GetSqlDbType<T>()
        {
            if (typeof(T) == typeof(Int32)
                || typeof(T) == typeof(int?))
            {
                return SqlDbType.Int;
            }

            if (typeof(T) == typeof(string))
            {
                return SqlDbType.NVarChar;
            }

            if (typeof(T) == typeof(Decimal))
            {
                return SqlDbType.Money;
            }

            if (typeof (T) == typeof (short)
                || typeof (T) == typeof (short?))
            {
                return SqlDbType.SmallInt;
            }

            if (typeof (T) == typeof (Guid)
                || typeof (T) == typeof (Guid?))
            {
                return SqlDbType.UniqueIdentifier;
            }

            if (typeof(T) == typeof(bool)
                || typeof(T) == typeof(bool?))
            {
                return SqlDbType.Bit;
            }

            if (typeof(T) == typeof(DateTime)
                || typeof(T) == typeof(DateTime?))
            {
                return SqlDbType.DateTime;
            }

            return SqlDbType.Image;
        }

        private void VerifyCommandText()
        {
            if (String.IsNullOrEmpty(_commandText))
            {
                throw new InvalidOperationException("Command Text has not been set.  Set command text before executing query.");
            }
        }

        private void ClearParameters()
        {
            _parameters.Clear();
        }
    }
}