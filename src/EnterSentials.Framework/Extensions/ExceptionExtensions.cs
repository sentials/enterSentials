using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public static class ExceptionExtensions
    {
        public static Guid GetHandlingInstanceId(this Exception exception, Guid fallbackId)
        {
            var id = default(Guid);
            return exception.Message.TryExtractGuid(out id) ? id : fallbackId;
        }

        public static Guid GetHandlingInstanceId(this Exception exception)
        { return exception.GetHandlingInstanceId(Guid.NewGuid()); }


        public static bool TryConsolidation(this IEnumerable<Exception> exceptions, out Exception exception)
        {
            exception = ((exceptions == null) || !exceptions.Any())
                ? null
                : (exceptions.Count() == 1) 
                    ? exceptions.First() 
                    : new AggregateException(exceptions);

            return exception != null;
        }
    }
}