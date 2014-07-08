using EnterSentials.Framework.Azure;
using EnterSentials.Framework.EmitMapper;
using EnterSentials.Framework.EntLib;
using EnterSentials.Framework.Newtonsoft;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.Unity;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer RegisterCoreFrameworkComponentThirdPartyImplementations(this IUnityContainer container)
        {
            return container
                .RegisterType<INewtonsoftJsonSerializerFactory, NaiveNewtonsoftJsonSerializerFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IJsonSerializer, NewtonsoftBasedJsonSerializer>()
                .RegisterType<IObjectMapperFactory, EmitMapperBasedObjectMapperFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IConfigurationSource>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => ConfigurationSourceFactory.Create()))
                .RegisterType<ExceptionPolicyFactory>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => new ExceptionPolicyFactory(c.Resolve<IConfigurationSource>())))
                .RegisterType<ExceptionManager>(new InjectionFactory(c => c.Resolve<ExceptionPolicyFactory>().CreateManager()))
                .RegisterType<IExceptionManager, EntLibBasedExceptionManager>()
                .RegisterType<EventListener, ObservableEventListener>()
                .RegisterType<ILoggingPipelineConfigurations, DefaultLoggingPipelineConfigurations>()
                .RegisterType<IAzureBlobStorageConnectionStringResolver, SettingsBasedAzureBlogStorageConnectionStringResolver>()
                .RegisterType<IFileRepositoryFactory, AzureFileRepositoryFactory>()
                ;
        }
    }
}
