using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using System;
using System.Diagnostics;
using System.IO;

namespace EnterSentials.Framework.EntLib.Logging.SLAB
{
    public sealed class DebugSink : IObserver<EventEntry>
    {
        private readonly IEventTextFormatter formatter = null;


        public DebugSink(IEventTextFormatter formatter = null)
        { this.formatter = formatter ?? new EventTextFormatter(); }

        public void OnNext(EventEntry entry)
        {
            if (entry != null)
            {
                var message = (string)null;

                using (var writer = new StringWriter())
                {
                    formatter.WriteEvent(entry, writer);
                    message = writer.ToString(); 
                }

                Debug.WriteLine(message);
            }
        }


        public void OnCompleted()
        { }

        public void OnError(Exception error)
        { }
    }
}
