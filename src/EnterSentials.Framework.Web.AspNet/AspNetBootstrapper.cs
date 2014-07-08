namespace EnterSentials.Framework.Web.AspNet
{
    public abstract class AspNetBootstrapper : IBootstrapper
    {
        protected virtual void ConfigureIocForInfrastructure()
        { }

        protected virtual void ConfigureDataAccessProviders()
        { }

        protected virtual void ConfigureVirtualPathProviders()
        { }

        protected virtual void ConfigureIocWithModulesLoaded()
        { }


        protected virtual void ConfigureAreas()
        {  }

        protected virtual void ConfigureWebApi()
        { }

        protected virtual void ConfigureFiltering()
        { }

        protected virtual void ConfigureStandardRouting()
        { }

        protected virtual void ConfigureBundling()
        { }

        protected virtual void ConfigureAuthorization()
        { }


        protected virtual void ConfigureMvcComponents()
        {
            ConfigureAreas();
            ConfigureWebApi();
            ConfigureFiltering();
            ConfigureStandardRouting();
            ConfigureBundling();
        }


        public virtual void Initialize()
        {
            ConfigureIocForInfrastructure();
            ConfigureDataAccessProviders();
            ConfigureVirtualPathProviders();
            ConfigureIocWithModulesLoaded();
            ConfigureMvcComponents();
            ConfigureAuthorization();
        }
    }
}
