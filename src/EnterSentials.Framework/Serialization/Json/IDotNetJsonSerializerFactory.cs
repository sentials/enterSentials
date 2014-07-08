using System.Web.Script.Serialization;

namespace EnterSentials.Framework
{
    public interface IDotNetJsonSerializerFactory
    {
        JavaScriptSerializer Get();
    }
}