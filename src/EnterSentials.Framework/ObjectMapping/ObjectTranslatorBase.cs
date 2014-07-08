using System;

namespace EnterSentials.Framework
{
    public abstract class ObjectTranslatorBase<TFrom, TTo> : IObjectTranslator<TFrom, TTo>, IObjectTranslator
    {
        private readonly Type fromType = typeof(TFrom);
        private readonly Type toType = typeof(TTo);

        public Type FromType { get { return fromType; } }
        public Type ToType { get { return toType; } }


        public abstract TTo Translate(TFrom from);


        public object Translate(object from)
        { return Translate((TFrom)from); }
    }
}