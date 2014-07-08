namespace EnterSentials.Framework
{
    public static class AssemblyNamingPolicy
    {
        public static bool IsProductionAssemblyName(this string assemblyName)
        {
            Guard.AgainstNullOrEmpty(assemblyName, "assemblyName");
            return !assemblyName.EndsWith(Naming.TestAssemblyNameSuffix);
        }

        public static bool IsProductAssemblyName(this string assemblyName)
        {
            Guard.AgainstNullOrEmpty(assemblyName, "assemblyName");
            return assemblyName.StartsWith(Naming.ProductAssemblyNamePrefix);
        }
    }
}