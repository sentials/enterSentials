using EnterSentials.Framework.Domain.EF;
using EnterSentials.Framework.SendGrid;
using Microsoft.Practices.Unity;

namespace EnterSentials.Framework
{
    public static class IUnityContainerExtensions
    {
        public static IUnityContainer RegisterServicesFrameworkComponentThirdPartyImplementations(this IUnityContainer container)
        {
            return container
                .RegisterType<IEmailDispatcher, SendGridEmailDispatcher>()
                .RegisterType<IRepositoryFactory, DomainContextBasedRepositoryFactory>()
                .RegisterType<IDomainContext, DbContextBasedDomainContext>()
                ;
        }
    }
}