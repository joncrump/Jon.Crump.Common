using System.Collections.Specialized;
using System.Configuration;

namespace Lmts.Framework.Configuration
{
    /// <summary>
    /// Summary description for DatabaseUtils
    /// </summary>
    public static class ConnectionStringManager
    {
        #region Fields

        private static NameValueCollection _connectionStrings = new NameValueCollection();

        #endregion

        #region Constructors

        static ConnectionStringManager()
        {
            PopulateConnectionStrings();
        }

        #endregion

        #region Properties

        public static NameValueCollection ConnectionStrings
        {
            get
            {
                if (_connectionStrings == null)
                    return new NameValueCollection();

                return _connectionStrings;
            }
        }

        #endregion

        #region Methods

        private static void PopulateConnectionStrings()
        {
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                if (_connectionStrings[connectionString.Name] == null)
                {
                    _connectionStrings.Add(connectionString.Name, connectionString.ConnectionString);
                }
            }
        }

        #endregion
    }
}