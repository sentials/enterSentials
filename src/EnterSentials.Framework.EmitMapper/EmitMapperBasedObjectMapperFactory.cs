using EmitMapper;
using EmitMapper.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EnterSentials.Framework.EmitMapper
{
    public class EmitMapperBasedObjectMapperFactory : IObjectMapperFactory
    {
        private readonly ICollection<TranslatorEntry> translatorEntries = new Collection<TranslatorEntry>();
        private readonly TypeDictionary<CopierEntry> copierEntries = new TypeDictionary<CopierEntry>();

        private readonly IComponents components = null;
        private DefaultMapConfig defaultShallowCloningMappingConfigurator = null;
        private DefaultMapConfig defaultDeepCloningMappingConfigurator = null;
        private DefaultMapConfig configuredShallowCloningMappingConfigurator = null;
        private DefaultMapConfig configuredDeepCloningMappingConfigurator = null;
        private ObjectMapperManager defaultShallowCloningMappers = null;
        private ObjectMapperManager defaultDeepCloningMappers = null;
        private ObjectMapperManager configuredShallowCloningMappers = null;
        private ObjectMapperManager configuredDeepCloningMappers = null;


        private static readonly MethodInfo GetCopierMethod = typeof(EmitMapperBasedObjectMapperFactory).GetMethod(
            Name.Of.Method.On<EmitMapperBasedObjectMapperFactory>(e => e.GetCopier<object, object>(default(SourceProperties), default (DefinedCopiers))),
            new Type[] { typeof(SourceProperties), typeof(DefinedCopiers) });

        private static readonly MethodInfo GetTranslatorMethod = typeof(EmitMapperBasedObjectMapperFactory).GetMethod(
            Name.Of.Method.On<EmitMapperBasedObjectMapperFactory>(e => e.GetTranslator<object, object>(default(SourceProperties), default(DefinedTranslators))),
            new Type[] { typeof(SourceProperties), typeof(DefinedTranslators) });


        private void Configure(DefaultMapConfig configurator, SourceProperties cloning)
        {
            if (cloning == SourceProperties.ByValueRecursively)
                configurator.DeepMap();
            else
                configurator.ShallowMap();

            configurator.ConvertGeneric(
                typeof(ICollection<>),
                typeof(IEnumerable<>),
                new DefaultCustomConverterProvider(cloning == SourceProperties.ByValueRecursively
                    ? typeof(DeepCloningICollectionToEnumerableConverterBase<,>)
                    : typeof(ShallowCloningICollectionToEnumerableConverterBase<,>)));
        }


        private void ConfigureConfigured(DefaultMapConfig configurator, MethodInfo convertUsingMethod, Delegate convertUsingDelegate)
        { convertUsingMethod.Invoke(configurator, new object[] { convertUsingDelegate }); }



        protected IEnumerable<DefaultMapConfig> GetDefaultConfigurators()
        {
            var configurators = new List<DefaultMapConfig>
            {
                new DefaultMapConfig(),
                new DefaultMapConfig()
            };

            var i = 0;
            foreach (var configurator in configurators)
                Configure(configurator, ((i++ % 2) == 0) ? SourceProperties.ByReference : SourceProperties.ByValueRecursively);

            return configurators;
        }


        private void InitializeDefaultMappingConfigurators()
        {
            var configurators = GetDefaultConfigurators();
            defaultShallowCloningMappingConfigurator = configurators.ElementAt(0);
            defaultDeepCloningMappingConfigurator = configurators.ElementAt(1);
            defaultShallowCloningMappers = new ObjectMapperManager();
            defaultDeepCloningMappers = new ObjectMapperManager();
        }


        private void RefreshConfiguredMappingConfigurators()
        {
            var configurators = GetDefaultConfigurators();

            foreach (var entry in translatorEntries)
            {
                var convertUsingMethod = entry.ConvertUsingMethod;
                var convertUsingDelegate = entry.ConvertUsingDelegate =
                    (Delegate) entry.GetConvertUsingDelegateMethod.Invoke(null, new object[] { entry.TranslatorType });

                foreach (var configurator in configurators)
                    ConfigureConfigured(configurator, convertUsingMethod, convertUsingDelegate);
            }

            configuredShallowCloningMappingConfigurator = configurators.ElementAt(0);
            configuredDeepCloningMappingConfigurator = configurators.ElementAt(1);
            configuredShallowCloningMappers = new ObjectMapperManager();
            configuredDeepCloningMappers = new ObjectMapperManager();
        }


        
        protected Type Sanitized(Type type)
        { return type.IsDynamicProxyType() ? type.BaseType : type; }


        private bool IsValidCopierType(Type type, out Type fromType, out Type toType)
        { return type.InheritsFromObjectCopierBase(out fromType, out toType); }

        private bool IsValidTranslatorType(Type type, out Type fromType, out Type toType)
        { return type.InheritsFromObjectTranslatorBase(out fromType, out toType); }


        public void RegisterCopiers(IEnumerable<Type> objectCopierTypes)
        {
            Guard.AgainstNull(objectCopierTypes, "objectCopierTypes");

            var entries = new Collection<CopierEntry>();

            foreach (var copierType in objectCopierTypes)
            {
                var fromType = (Type)null;
                var toType = (Type)null;
                var isValidCopierType = IsValidCopierType(copierType, out fromType, out toType);

                Guard.Against(
                    objectCopierTypes, 
                    t => !isValidCopierType,
                    "Every provided copier type must be not null and inherit from the required abstract class.",
                    "objectCopierTypes"
                );
                
                entries.Add(new CopierEntry(copierType, fromType, toType));
            }

            entries.ForEach(entry => copierEntries.Add(new[] { entry.FromType, entry.ToType }, entry));

            RefreshConfiguredMappingConfigurators();
        }
        

        public void RegisterCopier(Type objectCopierType)
        {
            Guard.AgainstNull(objectCopierType, "objectCopierType");
            RegisterCopiers(new Type[] { objectCopierType });
        }

        public void RegisterCopier<TObjectCopier>() where TObjectCopier : IObjectCopier
        { RegisterCopier(typeof(TObjectCopier)); }

        
        public IObjectCopier<TFrom, TTo> GetCopier<TFrom, TTo>(
            SourceProperties cloning = SourceProperties.ByReference, 
            DefinedCopiers with = DefinedCopiers.Included
        )
        {
            var copier = (IObjectCopier<TFrom, TTo>) null;
            var copierEntry = (CopierEntry)null;

            if (copierEntries.TryGetValue(new[] { typeof(TFrom), typeof(TTo)}, out copierEntry))
                copier = (IObjectCopier<TFrom, TTo>)Components.Instance.Get(copierEntry.CopierType);
            else
            {
                var mapper =
                    with == DefinedCopiers.Excluded
                        ? (cloning == SourceProperties.ByReference
                            ? defaultShallowCloningMappers.GetMapper<TFrom, TTo>(defaultShallowCloningMappingConfigurator)
                            : defaultDeepCloningMappers.GetMapper<TFrom, TTo>(defaultDeepCloningMappingConfigurator))
                        : (cloning == SourceProperties.ByReference
                            ? configuredShallowCloningMappers.GetMapper<TFrom, TTo>(configuredShallowCloningMappingConfigurator)
                            : configuredDeepCloningMappers.GetMapper<TFrom, TTo>(configuredDeepCloningMappingConfigurator));

                copier = new EmitMapperBasedObjectCopier<TFrom, TTo>(mapper);
            }

            return copier;
        }


        public IObjectCopier GetCopier(
            Type fromType, 
            Type toType, 
            SourceProperties cloning = SourceProperties.ByReference, 
            DefinedCopiers with = DefinedCopiers.Included
        )
        {
            return (IObjectCopier)GetCopierMethod
                .MakeGenericMethod(Sanitized(fromType), Sanitized(toType))
                .Invoke(this, new object[] { cloning, with }); 
        }


        public void RegisterTranslators(IEnumerable<Type> objectTranslatorTypes)
        {
            Guard.AgainstNull(objectTranslatorTypes, "objectTranslatorTypes");

            var entries = new Collection<TranslatorEntry>();

            foreach (var translatorType in objectTranslatorTypes)
            {
                var fromType = (Type) null;
                var toType = (Type) null;
                var isValidTranslatorType = IsValidTranslatorType(translatorType, out fromType, out toType);

                Guard.Against(
                    objectTranslatorTypes, 
                    t => !isValidTranslatorType, 
                    "Every provided translator type must be not null and inherit from the required abstract class.", 
                    "objectTranslatorTypes"
                );

                entries.Add(new TranslatorEntry(translatorType, fromType, toType));
            }

            entries.ForEach(translatorEntries.Add);

            RefreshConfiguredMappingConfigurators();
        }


        public void RegisterTranslator(Type objectTranslatorType)
        {
            Guard.AgainstNull(objectTranslatorType, "objectTranslatorType");
            RegisterTranslators(new Type[] { objectTranslatorType });
        }


        public void RegisterTranslator<TObjectTranslator>() where TObjectTranslator : IObjectTranslator
        { RegisterTranslator(typeof(TObjectTranslator)); }


        public IObjectTranslator<TFrom, TTo> GetTranslator<TFrom, TTo>(
            SourceProperties cloning = SourceProperties.ByReference, 
            DefinedTranslators with = DefinedTranslators.Included
        )
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(
                with == DefinedTranslators.Excluded
                    ? (cloning == SourceProperties.ByReference 
                        ? defaultShallowCloningMappingConfigurator 
                        : defaultDeepCloningMappingConfigurator)
                    : (cloning == SourceProperties.ByReference 
                        ? configuredShallowCloningMappingConfigurator 
                        : configuredDeepCloningMappingConfigurator));

            return new EmitMapperBasedObjectTranslator<TFrom, TTo>(mapper);
        }


        public IObjectTranslator GetTranslator(
            Type fromType, 
            Type toType, 
            SourceProperties cloning = SourceProperties.ByReference, 
            DefinedTranslators with = DefinedTranslators.Included
        )
        { 
            return (IObjectTranslator) GetTranslatorMethod
                .MakeGenericMethod(Sanitized(fromType), Sanitized(toType))
                .Invoke(this, new object[] { cloning, with }); 
        }
        

        public EmitMapperBasedObjectMapperFactory(IComponents components)
        { 
            Guard.AgainstNull(components, "components");
            this.components = components;

            InitializeDefaultMappingConfigurators();
            RefreshConfiguredMappingConfigurators(); 
        }


        private class CopierEntry
        {
            public Type CopierType { get; private set; }
            public Type FromType { get; private set; }
            public Type ToType { get; private set; }

            public CopierEntry(Type copierType, Type fromType, Type toType)
            {
                Guard.AgainstNull(copierType, "copierType");
                Guard.AgainstNull(fromType, "fromType");
                Guard.AgainstNull(toType, "toType");

                CopierType = copierType;
                FromType = fromType;
                ToType = toType;
            }
        }


        private class TranslatorEntry
        {
            private static readonly BindingFlags ConvertUsingBindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            private static readonly BindingFlags GetConvertUsingDelegateBindingFlags = BindingFlags.NonPublic | BindingFlags.Static;


            public Type TranslatorType { get; private set; }
            public MethodInfo ConvertUsingMethod { get; private set; }
            public MethodInfo GetConvertUsingDelegateMethod { get; private set; }
            public Delegate ConvertUsingDelegate { private get; set; }


            private static Func<TFrom, TTo> GetConvertUsingDelegate<TFrom, TTo>(Type translatorType)
            {
                return new Func<TFrom, TTo>(source =>
                    ((IObjectTranslator<TFrom, TTo>) Components.Instance.Get(translatorType)).Translate(source));
            }

            
            public TranslatorEntry(Type translatorType, Type fromType, Type toType)
            {
                Guard.AgainstNull(translatorType, "translatorType");
                Guard.AgainstNull(fromType, "fromType");
                Guard.AgainstNull(toType, "toType");

                TranslatorType = translatorType;

                ConvertUsingMethod = typeof(MapConfigBase<DefaultMapConfig>)
                    .GetMethod(Name.Of.Method.On<MapConfigBase<DefaultMapConfig>>(c => c.ConvertUsing<object, object>(null)), ConvertUsingBindingFlags)
                    .MakeGenericMethod(fromType, toType);

                GetConvertUsingDelegateMethod = typeof(TranslatorEntry)
                    .GetMethod("GetConvertUsingDelegate", GetConvertUsingDelegateBindingFlags)
                    .MakeGenericMethod(fromType, toType);
            }
        }


        private abstract class CollectionToEnumerableConverterBase<TFrom, TTo> : ICustomConverter
        {
            protected abstract SourceProperties Cloning { get; }

            private Type fromType = null;
            private Type toType = null;
            private MapConfigBaseImpl config = null;


            private TTo Map(TFrom from)
            {
                return (typeof(TFrom) == typeof(TTo)) && (Cloning == SourceProperties.ByReference)
                    ? (TTo)((object)from)
                    : ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(config).Map(from);
            }


            public IEnumerable<TTo> Convert(ICollection<TFrom> from, object state)
            {
                return (from == null)
                    ? null
                    : from.Select(Map).ToArray();
            }

            public void Initialize(Type from, Type to, MapConfigBaseImpl mappingConfig)
            {
                Guard.AgainstNull(from, "from");
                Guard.AgainstNull(to, "to");
                Guard.AgainstNull(mappingConfig, "mappingConfig");
                fromType = from;
                toType = to;
                config = mappingConfig;
            }
        }


        private class ShallowCloningICollectionToEnumerableConverterBase<TFrom, TTo> : CollectionToEnumerableConverterBase<TFrom, TTo>
        {
            protected override SourceProperties Cloning { get { return SourceProperties.ByReference; } }
        }


        private class DeepCloningICollectionToEnumerableConverterBase<TFrom, TTo> : CollectionToEnumerableConverterBase<TFrom, TTo>
        {
            protected override SourceProperties Cloning { get { return SourceProperties.ByValueRecursively; } }
        }
    }
}