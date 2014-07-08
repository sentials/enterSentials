using EnterSentials.Framework.EntLib.Logging.SLAB;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using System;

namespace EnterSentials.Framework.EntLib
{
    public static class ObservableEventEntryExtensions
    {
        public static SinkSubscription<DebugSink> LogToDebug(this IObservable<EventEntry> eventStream, IEventTextFormatter formatter = null)
        {
            var sink = new DebugSink(formatter);
            return new SinkSubscription<DebugSink>(eventStream.Subscribe(sink), sink);
        }


        public static SinkSubscription<WindowsEventLogSink> LogToWindowsEventLog(
            this IObservable<EventEntry> eventStream,
            string logName,
            string sourceName,
            string machineName = WindowsEventLogSink.DefaultMachineName,
            IEventTextFormatter formatter = null
        )
        {
            var sink = new WindowsEventLogSink(logName, sourceName, machineName, formatter);
            return new SinkSubscription<WindowsEventLogSink>(eventStream.Subscribe(sink), sink);
        }
    }
}
