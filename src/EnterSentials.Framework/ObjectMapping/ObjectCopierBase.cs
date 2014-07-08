using System;

namespace EnterSentials.Framework
{
    public abstract class ObjectCopierBase<TFrom, TTo> : IObjectCopier<TFrom, TTo>, IObjectCopier
    {
        private readonly Type fromType = typeof(TFrom);
        private readonly Type toType = typeof(TTo);

        public Type FromType { get { return fromType; } }
        public Type ToType { get { return toType; } }


        public abstract void Copy(TFrom from, TTo to);


        public void Copy(object from, object to)
        { Copy((TFrom)from, (TTo)to); }
    }
}