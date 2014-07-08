using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using System;
using System.Diagnostics;
using System.IO;

namespace EnterSentials.Framework.EntLib.Logging.SLAB
{
    public sealed class WindowsEventLogSink : ScopeLimitedObject, IObserver<EventEntry>
    {
        public const string DefaultMachineName = ".";

        private readonly IEventTextFormatter formatter = null;
        private EventLog eventLog = null;


        public void OnNext(EventEntry entry)
        {
            if (eventLog != null)
            {
                if (entry != null)
                {
                    var message = (string)null;

                    using (var writer = new StringWriter())
                    {
                        formatter.WriteEvent(entry, writer);
                        message = writer.ToString();
                    }

                    try
                    { eventLog.WriteEntry(message, entry.GetEventLogEntryType(), entry.EventId, entry.GetEventLogCategory()); }
                    catch // Don't let the app crash just because we couldn't automatically create the event log on the machine
                    { }
                }
            }
        }


        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }


        protected override void OnDisposeExplicit()
        {
            base.OnDisposeExplicit();
            eventLog.Dispose();
            eventLog = null;
        }


        public WindowsEventLogSink(string logName, string sourceName, string machineName = DefaultMachineName, IEventTextFormatter formatter = null)
        {
            Guard.AgainstNullOrEmpty(logName, "logName");
            Guard.AgainstNullOrEmpty(sourceName, "sourceName");
            Guard.AgainstNullOrEmpty(machineName, "machineName");

            this.formatter = formatter ?? new EventTextFormatter();

            //This code needs administrative privleges; the source should be created manually or by an install script for deployment scenarios.
            try
            {
                this.eventLog = new EventLog(logName, machineName, sourceName);
            
                if (!EventLog.SourceExists(sourceName))
                    EventLog.CreateEventSource(sourceName, logName);
            }
            catch // Don't let the app crash just because we couldn't automatically create the event log or entry on the machine
            { }
        }
    }
}
