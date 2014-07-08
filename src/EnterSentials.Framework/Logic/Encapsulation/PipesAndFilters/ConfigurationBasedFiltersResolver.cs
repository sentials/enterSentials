using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public class ConfigurationBasedFiltersResolver : IPipelineFiltersResolver
    {
        private readonly IApplicationConfiguration configuration = null;
        private readonly IUnitOfWork unitOfWork = null;


        private IFilterResolver GetFilterResolverFor(Type filterType)
        { return new UnitOfWorkBasedFilterResolver(unitOfWork, filterType); }


        public IEnumerable<IFilterResolver> GetFilterResolvers(string pipelineKey)
        {
            var pipeline = configuration.Pipelines.Pipelines[pipelineKey];
            return pipeline.Filters.Filters.Select(filter => GetFilterResolverFor(Type.GetType(filter.Type))).ToArray();
        }


        public ConfigurationBasedFiltersResolver(IApplicationConfiguration configuration, IUnitOfWork unitOfWork)
        {
            Guard.AgainstNull(configuration, "configuration");
            Guard.AgainstNull(unitOfWork, "unitOfWork");
            this.configuration = configuration;
            this.unitOfWork = unitOfWork;
        }
    }
}
