using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IObjectMapperFactory
    {
        void RegisterCopiers(IEnumerable<Type> objectCopierTypes);
        void RegisterCopier(Type objectCopierType);
        void RegisterCopier<TObjectCopier>() where TObjectCopier : IObjectCopier;

        IObjectCopier<TFrom, TTo> GetCopier<TFrom, TTo>(
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedCopiers with = DefinedCopiers.Included
        );

        IObjectCopier GetCopier(
            Type fromType, 
            Type toType, 
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedCopiers with = DefinedCopiers.Included
        );


        void RegisterTranslators(IEnumerable<Type> objectTranslatorTypes);
        void RegisterTranslator(Type objectTranslatorType);
        void RegisterTranslator<TObjectTranslator>() where TObjectTranslator : IObjectTranslator;

        IObjectTranslator<TFrom, TTo> GetTranslator<TFrom, TTo>(
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedTranslators with = DefinedTranslators.Included
        );

        IObjectTranslator GetTranslator(
            Type fromType,
            Type toType,
            SourceProperties cloning = SourceProperties.ByReference,
            DefinedTranslators with = DefinedTranslators.Included
        );
    }
}