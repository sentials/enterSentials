using System;

namespace EnterSentials.Framework
{
    public interface IObjectCopier
    {
        Type FromType { get; }
        Type ToType { get; }

        void Copy(object from, object to);
    }

    public interface IObjectCopier<TFrom, TTo> : IObjectCopier
    {
        void Copy(TFrom from, TTo to);
    }
}