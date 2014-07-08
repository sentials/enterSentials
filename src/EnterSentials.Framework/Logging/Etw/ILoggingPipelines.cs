using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework
{
    public interface ILoggingPipelines : IEnumerable<LoggingPipeline>, IDisposable
    { }
}