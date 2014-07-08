using EnterSentials.Framework.Unity;
using Microsoft.Practices.Unity;

namespace EnterSentials.Framework
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer RegisterCoreFrameworkComponents(this IUnityContainer container)
        {
            return container
                .RegisterType<IApplicationConfiguration, FileBasedApplicationConfiguration>()
                .RegisterType<IDotNetJsonSerializerFactory, NaiveDotNetJsonSerializerFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IJsonSerializer, DotNetBasedJsonSerializer>()
                .RegisterType<IDotNetXmlSerializerFactory, NaiveDotNetXmlSerializerFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IXmlSerializer, DotNetBasedXmlSerializer>()
                .RegisterType<IStringSerializer, JsonSerializerBasedStringSerializer>()
                .RegisterType<ICryptographer, SaltedHashingCryptographer>()
                .RegisterType<IPasswordManager, CryptographerBasedPasswordManager>()
                .RegisterType<ISsnManager, CryptographerBasedSsnManager>()
                .RegisterType<IExceptionManager, InertExceptionManager>()
                .RegisterType<IDomainLogicDispatcher, ExceptionManagingDomainLogicDispatcher>()
                .RegisterType<IDomainFactory, ComponentsBasedDomainFactory>()
                .RegisterType<IUnitOfWork, UnityBasedUnitOfWork>()
                .RegisterType<IUnitOfWorkFactory, UnityBasedUnitOfWorkFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IQueryFactory, ComponentsBasedQueryFactory>()
                .RegisterType<ICommandFactory, ComponentsBasedCommandFactory>()
                .RegisterType<IEventAggregator, ComponentsBasedEventAggregator>(new ContainerControlledLifetimeManager())
                .RegisterType<IExecutableBusinessObjectDispatcher, ExceptionManagingExecutableBusinessObjectDispatcher>()
                .RegisterType<IPipelineFiltersResolver, ConfigurationBasedFiltersResolver>()
                .RegisterType<IEventSubscribersResolver, ConfigurationBasedEventSubscribersResolver>()
                .RegisterType<IWebServiceClientFactory, WebServiceClientFactory>()
                .RegisterType<IEmailDispatcher, InertEmailDispatcher>()
                .RegisterType<IEmailQueue, DispatchCriteriaIgnorantEmailQueue>()
                .RegisterType<IEmailTemplateResolver, InertEmailTemplateResolver>()
                .RegisterType<IEmailGenerator, TemplateBasedEmailGenerator>()
                .RegisterType<LogEventSource>(new ContainerControlledLifetimeManager())
                .RegisterType<ILoggingSinkFactory, ActivatorBasedLoggingSinkFactory>()
                .RegisterType<ILoggingPipelineFactory, ComponentsBasedLoggingPipelineFactory>()
                .RegisterType<ILoggingPipelineConfigurations, EmptyLoggingPipelineConfigurations>()
                .RegisterType<ILoggingPipelines, ConfiguredLoggingPipelines>(new ContainerControlledLifetimeManager())
                .RegisterType<ILog, EventSourceLogAdapter>()
                .RegisterType<IBootstrapper, EventSubscribingBootstrapper>(typeof(EventSubscribingBootstrapper).Name)
                .RegisterType<IBootstrapper, ObjectTranslatorRegisteringBootstrapper>(typeof(ObjectTranslatorRegisteringBootstrapper).Name)
                .RegisterType<IBootstrapper, LoggingConfigurationBootstrapper>(typeof(LoggingConfigurationBootstrapper).Name)
                ;
        }
    }
}