using System;
using System.Linq;

namespace EnterSentials.Framework
{
    public class UnitOfWorkBasedFilterResolver : IFilterResolver
    {
        private class FilterAdapter : IFilter
        {
            private readonly Type filterType = null;
            private readonly IUnitOfWork unitOfWork = null;


            public TTarget Process<TTarget>(TTarget target)
            {
                var result = target;

                unitOfWork.Add(target);

                Guard.Against(
                    !((filterType.Implements<IFilter>()) || (filterType.Implements<IExecutableObject>())),
                    "Provided object must either be a filter or an executable object."
                );

                var filter = (filterType.Implements<ICommand>())
                    ? unitOfWork.Commands.Get(filterType)
                    : unitOfWork.Get(filterType);

                if (filter is IFilter)
                    unitOfWork.Add(result = ((IFilter)filter).Process(target));
                else
                {
                    Guard.Against(!(filter is IExecutableObject), "Resolved object must be an executable object to be adapted to a filter.");
                    ((IExecutableObject)filter).Execute();
                    var execution = unitOfWork.Get<IExecutionOutcome>();
                    if (execution.Failed)
                    {
                        Guard.Against(!execution.Exceptions.Any(), "Exceptions must be available if execution of a filter fails.");
                        if (execution.Exceptions.Count() == 1)
                            throw execution.Exceptions.First();
                        else
                            throw new AggregateException(execution.Exceptions);
                    }
                }

                return unitOfWork.Get<TTarget>();
            }


            public FilterAdapter(Type filterType, IUnitOfWork unitOfWork)
            {
                Guard.AgainstNull(filterType, "filterType");
                Guard.AgainstNull(unitOfWork, "unitOfWork");

                this.filterType = filterType;
                this.unitOfWork = unitOfWork;
            }
        }


        private readonly IUnitOfWork unitOfWork = null;
        private readonly Type filterType = null;


        public IFilter Get()
        { return new FilterAdapter(filterType, unitOfWork); }


        public UnitOfWorkBasedFilterResolver(IUnitOfWork unitOfWork, Type filterType)
        {
            Guard.AgainstNull(unitOfWork, "unitOfWork");
            Guard.AgainstNull(filterType, "filterType");

            this.unitOfWork = unitOfWork;
            this.filterType = filterType;
        }
    }
}
