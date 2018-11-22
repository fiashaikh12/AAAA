using Entities;
using Interface;
using Repository;
using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mail;

namespace Repository
{
    public class EmailerRepository : IEmailer
    {
        private static readonly object padlock = new object();
        private readonly string _email = ConfigurationManager.AppSettings["EmailFrom"].ToString();
        //private readonly string _password = ConfigurationManager.AppSettings["EmailPassword"].ToString();
        private static EmailerRepository _instance = null;
        public static EmailerRepository GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EmailerRepository();
                        }
                    }
                }
                return _instance;
            }
        }
        public bool Send(string emailAddress,string password)
        {
            bool emailSend = false;
            try
            {
                if (!string.IsNullOrEmpty(emailAddress))
                {
                    MailMessage objMail = new MailMessage
                    {
                        From = _email,
                        To = emailAddress,
                        Subject = "Forgot password",
                        BodyFormat = MailFormat.Html,
                        Priority = MailPriority.High,
                        Body = $"Your password is {password}"
                    };

                    SmtpMail.SmtpServer = "relay-hosting.secureserver.net";
                    SmtpMail.Send(objMail);
                    emailSend = true;
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return emailSend;
        }

        public Task SendAsync(string emailAddress)
        {
            throw new NotImplementedException();
        }
    }
}
