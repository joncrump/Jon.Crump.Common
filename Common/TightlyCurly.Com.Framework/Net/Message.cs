using System;
using TightlyCurly.Com.Framework;
using TightlyCurly.Com.Framework.Net;

namespace TightlyCurly.Com.Framework.Net
{
    public class Message
    {
        #region Fields

        private MessageCredentials _credentials = null;
        private MessageFormat _messageFormat = MessageFormat.Html;
        private string _subject = String.Empty;
        private string _body = String.Empty;
        private string _from = String.Empty;
        private string _to = String.Empty;
        private string _cc = String.Empty;
        private string _bcc = String.Empty;
        private string _replyTo = String.Empty;

        #endregion

        #region Constructors

        public Message()
        {
            
        }

        public Message(MessageCredentials credentials)
        {
            _credentials = credentials;
        }

        public Message(string to, string from, string subject, string body, MessageCredentials credentials)
            : this(credentials)
        {
            _to = to;
            _from = from;
            _subject = subject;
            _body = body;
        }

        public Message(string to, string from, string subject, string body, 
                       MessageFormat MessageFormat, MessageCredentials credentials) : this(to, from, subject, body, credentials)
        {
            _messageFormat = MessageFormat;
        }

        #endregion

        #region Properties

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        public string To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
        }

        public string Cc
        {
            get
            {
                return _cc;
            }
            set
            {
                _cc = value;
            }
        }

        public string Bcc
        {
            get
            {
                return _bcc;
            }
            set
            {
                _bcc = value;
            }
        }

        public string ReplyTo
        {
            get
            {
                return _replyTo;
            }
            set
            {
                _replyTo = value;
            }
        }

        public MessageFormat MessageFormat
        {
            get
            {
                return _messageFormat;
            }
            set
            {
                _messageFormat = value;
            }
        }

        public MessageCredentials Credentials
        {
            get
            {
                return _credentials;
            }
            set
            {
                _credentials = value;
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}