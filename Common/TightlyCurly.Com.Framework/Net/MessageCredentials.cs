using System;
using System.Collections.Generic;
using System.Text;
using TightlyCurly.Com.Framework;

namespace TightlyCurly.Com.Framework.Net
{
    public class MessageCredentials
    {
        #region Fields

        private bool _requiresSsl = false;
        private string _server = String.Empty;
        private string _username = String.Empty;
        private string _password = String.Empty;
        private int _port = 0;

        #endregion

        #region Constructors

        public MessageCredentials()
        {
            
        }

        public MessageCredentials(string server, string username, string password)
        {
            _server = server;
            _username = username;
            _password = password;
        }

        public MessageCredentials(string server, string username, string password, int port, bool requiresSsl)
            : this(server, username, password)
        {
            _port = port;
            _requiresSsl = requiresSsl;
        }

        #endregion

        #region Properties

        public string Server
        {
            get
            {
                return _server;
            }
            set
            {
                _server = value;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        public bool RequiresSsl
        {
            get
            {
                return _requiresSsl;
            }
            set
            {
                _requiresSsl = value;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}
