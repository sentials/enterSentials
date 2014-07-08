namespace EnterSentials.Framework
{
    public interface IEmailQueue
    {
        void Add(EmailQueueEntry entry);
    }
}