namespace EnterSentials.Framework
{
    public interface IEventPublishingBuffer
    {
        void Add<TEvent>(TEvent @event);
        void PublishAll();
    }
}