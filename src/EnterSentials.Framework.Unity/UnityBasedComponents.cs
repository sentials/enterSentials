using EnterSentials.Framework.Unity.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace EnterSentials.Framework.Unity
{
    public class UnityBasedComponents : ComponentsUnityAdapter
    {
        public static IUnityContainer NewUnityContainer()
        {
            var container = new UnityContainer().RegisterType<IComponents, ComponentsUnityAdapter>();

            var configurationSection = ConfigurationManager.GetSection(Settings.Default.UnityConfigurationSectionName) as UnityConfigurationSection;
            if (configurationSection != null)
                configurationSection.Configure(container);
            
            return container;
        }

        public UnityBasedComponents() : base(NewUnityContainer())
        { }
    }
}
