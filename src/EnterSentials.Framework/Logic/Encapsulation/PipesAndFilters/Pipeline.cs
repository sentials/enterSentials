namespace EnterSentials.Framework
{
    public abstract class Pipeline<TTarget> : UnitOfWorkBasedCommand<TTarget, TTarget>
    {
        private readonly IPipelineFiltersResolver filtersResolver = null;
        

        protected override TTarget ExecuteLogicWithResult()
        {
            var filterResolvers = filtersResolver.GetFilterResolvers(this.GetType().GetConfigurationKey());
            var context = Params;
            foreach (var filterResolver in filterResolvers)
                context = filterResolver.Get().Process(context);
            return context;
        }


        public Pipeline(IPipelineFiltersResolver filtersResolver, TTarget parameters, IUnitOfWork uow) : base(parameters, uow)
        {
            Guard.AgainstNull(filtersResolver, "filtersResolver");
            this.filtersResolver = filtersResolver;
        }
    }
}