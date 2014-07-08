using System;
namespace EnterSentials.Framework
{
    public interface IStringSerializer
    {
        string Serialize<T>(T @object);

        object Deserialize(string @string, Type targetType);
        T Deserialize<T>(string @string);
    }
}