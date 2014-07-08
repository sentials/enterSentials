using EnterSentials.Framework.Properties;

namespace EnterSentials.Framework
{
    public class DispatchCriteriaIgnorantEmailQueue : IEmailQueue
    {
        private static readonly string ErrorMessageFormat = Resources.CouldNotSendEmailErrorMessageFormat;

        private readonly IEmailDispatcher emailDispatcher = null;


        public void Add(EmailQueueEntry entry)
        {
            Guard.AgainstNull(entry, "entry");

            Guard.Against(
                !emailDispatcher.TrySend(entry.Email),
                string.Format(ErrorMessageFormat, entry.Email.Subject, entry.Email.ToAddress));
        }


        public DispatchCriteriaIgnorantEmailQueue(IEmailDispatcher emailDispatcher)
        {
            Guard.AgainstNull(emailDispatcher, "emailDispatcher");
            this.emailDispatcher = emailDispatcher;
        }
    }
}