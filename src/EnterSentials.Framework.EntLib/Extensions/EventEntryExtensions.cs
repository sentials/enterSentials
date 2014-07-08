using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System.Diagnostics;

namespace EnterSentials.Framework.EntLib
{
    public static class EventEntryExtensions
    {
        public static EventLogEntryType GetEventLogEntryType(this EventEntry eventEntry)
        {
            Guard.AgainstNull(eventEntry, "eventEntry");
            Guard.AgainstNull(eventEntry.Schema, "eventEntry.Schema");
            return eventEntry.Schema.Level.ToEventLogEntryType();
        }

        public static short GetEventLogCategory(this EventEntry eventEntry)
        {
            Guard.AgainstNull(eventEntry, "eventEntry");
            Guard.AgainstNull(eventEntry.Schema, "eventEntry.Schema");
            return (short) eventEntry.Schema.Task;
        }
    }
}