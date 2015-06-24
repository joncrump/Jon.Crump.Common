using System;
using System.Net.Mail;

namespace TightlyCurly.Com.Framework.Net
{
    public static class EmailManager
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool SendEmail(Message message)
        {
            var mailMessage = new MailMessage(
                message.From, message.To, message.Subject, message.Body);
            
            if (!String.IsNullOrEmpty(message.Cc))
            {
                mailMessage.CC.Add(message.Cc);
            }

            if (!String.IsNullOrEmpty(message.Bcc))
            {
                mailMessage.Bcc.Add(message.Bcc);
            }

            if (!String.IsNullOrEmpty(message.ReplyTo))
            {
                mailMessage.ReplyTo = new MailAddress(message.ReplyTo);
            }

            mailMessage.IsBodyHtml = (message.MessageFormat == MessageFormat.Html);

            var emailClient = new SmtpClient(message.Credentials.Server);
            var smtpCredentials = 
                new System.Net.NetworkCredential(message.Credentials.Username, message.Credentials.Password);
            emailClient.UseDefaultCredentials = false;
            emailClient.EnableSsl = message.Credentials.RequiresSsl;
            emailClient.Credentials = smtpCredentials;
            
            if (message.Credentials.Port > 0)
            {
                emailClient.Port = message.Credentials.Port;
            }

            emailClient.Send(mailMessage);

            return true;
        }

        #endregion
    }
}