namespace EnterSentials.Framework.Messaging
{
    public interface ITopicSubscription
    {
        string Topic
        { get; }

        void OnReceive(IMessage message);
    }
}