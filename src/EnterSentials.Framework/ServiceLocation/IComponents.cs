using System;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IComponents
    {
        IEnumerable<T> GetAll<T>();
        IEnumerable<object> GetAll(Type type);
        T Get<T>(string key);
        T Get<T>();
        object Get(Type type, string key);
        object Get(Type type);
    }
}