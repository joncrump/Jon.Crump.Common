using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Moq;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common
{
    public class MockDataReader : IDataReader, IEquatable<MockDataReader>
    {
        private bool _isClosed;
        private int _currentRow = -1;
        private readonly IDictionary<int, ColumnInfo> _results; 
        private readonly int _maxRows = 0;
        public IList<string> InvokedColumns { get; private set; } 

        public MockDataReader()
        {
            InvokedColumns = new List<string>();
        }

		public MockDataReader(DataContainer container) : this()
		{
		    if (container == null)
		    {
		        return;
		    }

		    _maxRows = container.NumberOfRecords;
		    _results = PopulateResults(container.Results);
		}

        public virtual void Dispose()
        {
            _isClosed = true;
        }

        public virtual string GetName(int i)
        {
            CheckValidIndex(i);

            return _results.First(index => index.Key == i).Value.ColumnName;
        }

        public virtual string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
            //CheckValidIndex(i);

            //return _results.First(index => index.Key == i).Value.Item2;
        }

        public virtual Type GetFieldType(int i)
        {
            throw new NotImplementedException();
            //CheckValidIndex(i);

            //return _results.First(index => index.Key == i).Value.Item3;
        }

        public virtual object GetValue(int i)
        {
            CheckValidIndex(i);

            var values = ExtractAndVerifyValues(i);

            return values[_currentRow];
        }

        public virtual int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public virtual int GetOrdinal(string name)
        {
            Guard.EnsureIsNotNullOrEmpty("name", name);

            if (InvokedColumns.All(s => String.CompareOrdinal(s, name) != 0))
            {
                InvokedColumns.Add(name);
            }

            try
            {
                return _results.First(r => r.Value.ColumnName == name).Key;
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("Sequence contains no matching element"))
                {
                    throw new InvalidOperationException("Could not locate ordinal for name {0}"
                        .FormatString(name));
                }

                throw;
            }
        }

        public virtual bool GetBoolean(int i)
        {
            return GetValueAtCurrentRowForIndex<bool>(i);
        }

        public virtual byte GetByte(int i)
        {
            return GetValueAtCurrentRowForIndex<byte>(i);
        }

        public virtual long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public virtual char GetChar(int i)
        {
            return GetValueAtCurrentRowForIndex<char>(i);
        }

        public virtual long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public virtual Guid GetGuid(int i)
        {
            return GetValueAtCurrentRowForIndex<Guid>(i);
        }

        public virtual short GetInt16(int i)
        {
            return GetValueAtCurrentRowForIndex<short>(i);
        }

        public virtual int GetInt32(int i)
        {
            return GetValueAtCurrentRowForIndex<int>(i);
        }

        public virtual long GetInt64(int i)
        {
            return GetValueAtCurrentRowForIndex<long>(i);
        }

        public virtual float GetFloat(int i)
        {
            return GetValueAtCurrentRowForIndex<float>(i);
        }

        public virtual double GetDouble(int i)
        {
            return GetValueAtCurrentRowForIndex<double>(i);
        }

        public virtual string GetString(int i)
        {
            return GetValueAtCurrentRowForIndex<string>(i);
        }

        public virtual decimal GetDecimal(int i)
        {
            return GetValueAtCurrentRowForIndex<decimal>(i);
        }

        public virtual DateTime GetDateTime(int i)
        {
            return GetValueAtCurrentRowForIndex<DateTime>(i);
        }

        public virtual IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsDBNull(int i)
        {
            return IsNull(i);
        }

        public virtual int FieldCount
        {
            get
            {
                return _results.IsNullOrEmpty() 
                    ? 0 
                    : _results.Count();
            }
        }

        object IDataRecord.this[int i]
        {
            get
            {
                return GetValueAtCurrentRowForIndex<object>(i);
            }
        }

        object IDataRecord.this[string name]
        {
            get
            {
                var ordinal = GetOrdinal(name);

                return GetValueAtCurrentRowForIndex<object>(ordinal);
            }
        }

        public virtual void Close()
        {
            _isClosed = true;
        }

        public virtual DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public virtual bool NextResult()
        {
            throw new NotImplementedException("Multiple result sets are not supported.");
        }

        public virtual bool Read()
        {
            if (_currentRow < _maxRows - 1)
            {
                _currentRow++;
                return true;
            }

            return false;
        }

        public virtual int Depth { get; private set; }
        public virtual bool IsClosed
        {
            get
            {
                return _isClosed;
            }
        }

        public virtual int RecordsAffected { get; private set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as MockDataReader);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(MockDataReader other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return true;
        }

        private IDictionary<int, ColumnInfo> PopulateResults(IEnumerable<ColumnInfo> columns)
        {
            if (columns.IsNullOrEmpty())
            {
                return new Dictionary<int, ColumnInfo>();
            }

            var results = new Dictionary<int, ColumnInfo>();
            var index = 0;

            foreach (var column in columns)
            {
                results.Add(index, column);

                index++;
            }

            return results;
        }

        private void CheckValidIndex(int i)
        {
            if (_results.IsNullOrEmpty() || !_results.ContainsKey(i))
            {
                throw new IndexOutOfRangeException("The specified index does not exist.");
            }
        }

        private object[] ExtractAndVerifyValues(int i)
        {
            object[] values = null;

            if (IsClosed)
            {
                throw new InvalidOperationException("Cannot get data.  The DataReader is closed.");
            }

            if (_currentRow < 0)
            {
                throw new InvalidOperationException("You must invoke the Read method before accessing.");
            }

            if (_currentRow <= (_maxRows - 1))
            {
                values = _results.First(index => index.Key == i).Value.Values.ToSafeArray();
            }

            return values;
        }

        private T GetValueAtCurrentRowForIndex<T>(int i)
        {
            CheckValidIndex(i);

            var values = ExtractAndVerifyValues(i);

            var value = values[_currentRow];

            if (value == null)
            {
                return default(T);
            }

            return (T)value;
        }

        private bool IsNull(int i)
        {
            CheckValidIndex(i);

            var values = ExtractAndVerifyValues(i);
            object value = null;

            value = values[_currentRow];

            return value == null;
        }
    }
}
