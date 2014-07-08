using EnterSentials.Framework.Properties;
using System.Web.Script.Serialization;

namespace EnterSentials.Framework
{
    public class NaiveDotNetJsonSerializerFactory : IDotNetJsonSerializerFactory
    {
        private static readonly int MaxJsonLength = Settings.Default.DotNetJsonSerializerMaxJsonLength;


        public JavaScriptSerializer Get()
        { return new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }; }
    }
}