using System;

namespace EnterSentials.Framework
{
    public interface IObjectTranslator
    {
        Type FromType { get; }
        Type ToType { get; }

        object Translate(object from);
    }

    public interface IObjectTranslator<TFrom, TTo> : IObjectTranslator
    {
        TTo Translate(TFrom from);
    }
}