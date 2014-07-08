using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public class ExecutePipeline<TContext> : UnitOfWorkBasedCommand<TContext, ExecutePipeline<TContext>.Parameters>
    {
        private readonly IPipelineFiltersResolver filtersResolver = null;

        public class Parameters
        {
            [RequiredAndNonDefaultNorEmpty]
            public string PipelineKey { get; set; }

            [Required]
            public TContext Context { get; set; }
        }


        protected override TContext ExecuteLogicWithResult()
        { 
            var filterResolvers = filtersResolver.GetFilterResolvers(Params.PipelineKey);
            var context = Params.Context;
            foreach (var filterResolver in filterResolvers)
                context = filterResolver.Get().Process(context);
            return context;
        }


        public ExecutePipeline(IPipelineFiltersResolver filtersResolver, Parameters parameters, IUnitOfWork uow) : base(parameters, uow)
        {
            Guard.AgainstNull(filtersResolver, "filtersResolver");
            this.filtersResolver = filtersResolver;
        }
    }
}
