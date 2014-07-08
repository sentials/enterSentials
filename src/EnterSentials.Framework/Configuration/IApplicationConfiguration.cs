namespace EnterSentials.Framework
{
    public interface IApplicationConfiguration
    {
        PipelinesConfigurationElement Pipelines { get; }
        EventsConfigurationElement Events { get; }
    }
}