using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TightlyCurly.Com.Framework.Extensions
{
    public static class DataReaderExtensions
    {
        private static IDictionary<Type, Func<IDataReader, string, object>> _mappings =
            new Dictionary<Type, Func<IDataReader, string, object>>
            {
                { typeof(bool), (r, n) => !r.IsDBNull(r.GetOrdinal(n)) && Boolean.Parse(r[n].ToString()) },
                { typeof(bool?), (r, n) =>
                                 {
                                     if (r.IsDBNull(r.GetOrdinal(n)))
                                     {
                                         return null;
                                     }

                                     return (bool?)Boolean.Parse(r[n].ToString());
                                 }},
                { typeof(byte), (r, n) =>
                                {
                                    if (r.IsDBNull(r.GetOrdinal(n)))
                                    {
                                        return default(byte);
                                    }

                                    return (byte)r[n];
                                }},
                { typeof(byte?), (r, n) =>
                                 {
                                     if (r.IsDBNull(r.GetOrdinal(n)))
                                     {
                                         return null;
                                     }

                                     return (byte?)r[n];
                                 }},
                { typeof(char), (r, n) =>
                                {
                                    if (r.IsDBNull(r.GetOrdinal(n)))
                                    {
                                        return default(char);
                                    }

                                    return (char)r[n];
                                }},
                { typeof (char?), (r, n) =>
                                  {
                                      if (r.IsDBNull(r.GetOrdinal(n)))
                                      {
                                          return null;
                                      }

                                      return (char?)r[n];
                                  }},
                { typeof(Guid), (r, n) =>
                                {
                                    if (r.IsDBNull(r.GetOrdinal(n)))
                                    {
                                        return default(Guid);
                                    }

                                    return (Guid)r[n];
                                }},
                { typeof(Guid?), (r, n) =>
                                 {
                                     if (r.IsDBNull(r.GetOrdinal(n)))
                                     {
                                         return null;
                                     }

                                     return (Guid?)r[n];
                                 }},
                { typeof(short), (r, n) =>
                                 {
                                     if (r.IsDBNull(r.GetOrdinal(n)))
                                     {
                                         return default(short);
                                     }

                                     return (short)r[n];
                                 }},
                { typeof(short?), (r, n) =>
                                  {
                                      if (r.IsDBNull(r.GetOrdinal(n)))
                                      {
                                          return null;
                                      }

                                      return (short?)r[n];
                                  }},
                { typeof(int), (r, n) =>
                               {
                                   if (r.IsDBNull(r.GetOrdinal(n)))
                                   {
                                       return default(int);
                                   }

                                   return Int32.Parse(r[n].ToString());
                               }},
                { typeof(int?), (r, n) =>
                                {
                                    if (r.IsDBNull(r.GetOrdinal(n)))
                                    {
                                        return null;
                                    }

                                    return (int?)r[n];
                                }},
                { typeof(string), (r, n) =>
                                  {
                                      if (r.IsDBNull(r.GetOrdinal(n)))
                                      {
                                          return null;
                                      }

                                      return r[n].ToString();
                                  }},
                { typeof(long), (r, n) =>
                                {
                                    if (r.IsDBNull(r.GetOrdinal(n)))
                                    {
                                        return default(long);
                                    }

                                    return Int64.Parse(r[n].ToString());
                                }},
                { typeof(long?), (r, n) =>
                                 {
                                     if (r.IsDBNull(r.GetOrdinal(n)))
                                     {
                                         return null;
                                     }

                                     return (long?)r[n];
                                 }},
                { typeof(float), (r, n) =>
                                 {
                                     if (r.IsDBNull(r.GetOrdinal(n)))
                                     {
                                         return default(float);
                                     }

                                     return (float)r[n];
                                 }},
                { typeof(float?), (r, n) =>
                                  {
                                      if (r.IsDBNull(r.GetOrdinal(n)))
                                      {
                                          return null;
                                      }

                                      return (float?)r[n];
                                  }},
                { typeof(double), (r, n) =>
                                  {
                                      if (r.IsDBNull(r.GetOrdinal(n)))
                                      {
                                          return default(double);
                                      }

                                      return (double)r[n];
                                  }},
                { typeof(double?), (r, n) =>
                                   {
                                       if (r.IsDBNull(r.GetOrdinal(n)))
                                       {
                                           return null;
                                       }

                                       return (double?)r[n];
                                   }},
                { typeof(decimal), (r, n) =>
                                   {
                                       if (r.IsDBNull(r.GetOrdinal(n)))
                                       {
                                           return default(decimal);
                                       }

                                       return (decimal)r[n];
                                   }},
                { typeof(decimal?), (r, n) =>
                                    {
                                        if (r.IsDBNull(r.GetOrdinal(n)))
                                        {
                                            return null;
                                        }

                                        return (decimal?)r[n];
                                    }},
                { typeof(DateTime), (r, n) =>
                                    {
                                        if (r.IsDBNull(r.GetOrdinal(n)))
                                        {
                                            return default(DateTime);
                                        }

                                        return DateTime.Parse(r[n].ToString());
                                    }},
                { typeof(DateTime?), (r, n) =>
                                     {
                                         if (r.IsDBNull(r.GetOrdinal(n)))
                                         {
                                             return null;
                                         }

                                         return (DateTime?)r[n];
                                     }},
                { typeof(Uri), (r, n) =>
                    {
                        if (r.IsDBNull(r.GetOrdinal(n)))
                        {
                            return null;
                        }

                        return new Uri(r[n].ToString());
                    }}
            }; 

        public static T Get<T>(this IDataReader reader, string columnName)
        {
            return (T)Get(reader, typeof(T), columnName);
        }

        public static object Get(this IDataReader reader, Type type, string columnName)
        {
            return _mappings[type](reader, columnName);
        }

        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return false;
            }

            try
            {
                reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
                return (reader.GetSchemaTable().DefaultView.Count > 0);
            }
            catch
            {
                return false;
            }
        }

        public static bool HasValue(this IDataReader reader, string columnName)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return false;
            }

            return reader[columnName] != DBNull.Value;
        }

        public static IEnumerable<IDictionary<string, object>> ToDictionary(this IDataReader reader)
        {
            if (reader == null)
            {
                return new List<Dictionary<string, object>>();
            }

            var values = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var valuesDictionary = new Dictionary<string, object>();

                for (var column = 0; column < reader.FieldCount; column++)
                {
                    valuesDictionary.Add(reader.GetName(column), reader.GetValue(column));
                }

                values.Add(valuesDictionary);
            }

            return values;
        }
    }
}
