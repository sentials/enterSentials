namespace EnterSentials.Framework
{
    public static class IComponentsExtensions
    {
        public static IComponents ExecuteBootstrappers(this IComponents components)
        {
            components.GetAll<IBootstrapper>().ForEach(bootstapper => bootstapper.Initialize());
            return components;
        }
    }
}
