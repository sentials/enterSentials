using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnterSentials.Framework
{
    public static class AppDomainExtensions
    {
        public static IEnumerable<Assembly> GetProductionAssemblies(this AppDomain appDomain)
        { return appDomain.GetAssemblies().Where(assembly => assembly.IsProductionAssembly()); }

        public static IEnumerable<Assembly> GetProductAssemblies(this AppDomain appDomain)
        { return appDomain.GetAssemblies().Where(assembly => assembly.IsProductAssembly()); }

        public static IEnumerable<Assembly> GetProductProductionAssemblies(this AppDomain appDomain)
        { return appDomain.GetAssemblies().Where(assembly => assembly.IsProductAssembly() && assembly.IsProductionAssembly()); }
    }
}
