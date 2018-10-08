using Entities;
using Interface;
using Repository;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class EmailerRepository : IEmailer
    {
        private static readonly object padlock = new object();
        private readonly string _email = ConfigurationManager.AppSettings["EmailFrom"].ToString();
        private readonly string _password = ConfigurationManager.AppSettings["EmailPassword"].ToString();
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
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(_email);
                        mail.To.Add(emailAddress);
                        mail.Subject = "Forgot password";
                        mail.Body = $"Your password is {password}";
                        mail.IsBodyHtml = false;
                        using (SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25))
                        {
                            client.Credentials = new NetworkCredential(_email, _password);
                            client.EnableSsl = true;
                            client.Send(mail);
                            emailSend = true;
                        }
                    }
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
