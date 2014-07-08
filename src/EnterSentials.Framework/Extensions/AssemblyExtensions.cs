using System.Reflection;

namespace EnterSentials.Framework
{
    public static class AssemblyExtensions
    {
        public static bool IsProductionAssembly(this Assembly assembly)
        { return (assembly != null) && assembly.GetName().Name.IsProductionAssemblyName(); }

        public static bool IsProductAssembly(this Assembly assembly)
        { return (assembly != null) && assembly.GetName().Name.IsProductAssemblyName(); }
    }
}
