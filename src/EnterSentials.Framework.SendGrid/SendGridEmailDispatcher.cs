using EnterSentials.Framework.SendGrid.Properties;
using System.Net;
using System.Net.Mail;
using SendGridMail = SendGrid.Mail;
using SendGridTransport = SendGrid.Transport;

namespace EnterSentials.Framework.SendGrid
{
    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        private static readonly string UserName = Settings.Default.SendGridUserName;
        private static readonly string Password = Settings.Default.SendGridPassword;


        public bool TrySend(Email email)
        {
            var wasSuccessful = false;

            try
            {
                var message = SendGridMail.GetInstance();
                message.From = new MailAddress(email.FromAddress, email.FromDisplayName);
                message.AddTo(email.ToAddress);
                message.Subject = email.Subject;
                message.Text = email.Body;

                var transportWeb = SendGridTransport.Web.GetInstance(new NetworkCredential(UserName, Password));
                transportWeb.DeliverAsync(message).Wait();
                wasSuccessful = true;
            }
            catch
            { }

            return wasSuccessful;
        }
    }
}
