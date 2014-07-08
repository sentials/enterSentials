using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public interface IPipelineFiltersResolver
    {
        IEnumerable<IFilterResolver> GetFilterResolvers(string pipelineKey);
    }
}