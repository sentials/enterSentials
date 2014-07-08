using System;
using System.Linq;

namespace EnterSentials.Framework
{
    public class ObjectTranslatorRegisteringBootstrapper : IBootstrapper
    {
        private readonly IObjectMapperFactory translatorFactory = null;

        public void Initialize()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetProductAssemblies())
                foreach (var translatorType in assembly.GetTypes().Where(t => !t.IsAbstract && t.InheritsFromObjectTranslatorBase()))
                    translatorFactory.RegisterTranslator(translatorType);
        }

        public ObjectTranslatorRegisteringBootstrapper(IObjectMapperFactory translatorFactory)
        {
            Guard.AgainstNull(translatorFactory, "translatorFactory");
            this.translatorFactory = translatorFactory;
        }
    }
}