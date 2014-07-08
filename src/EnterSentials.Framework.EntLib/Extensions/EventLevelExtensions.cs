using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace EnterSentials.Framework.EntLib
{
    public static class EventLevelExtensions
    {
        public static EventLogEntryType ToEventLogEntryType(this EventLevel eventLevel)
        {
            Guard.AgainstNull(eventLevel, "eventLevel");
            return (eventLevel == EventLevel.Critical) || (eventLevel == EventLevel.Error)
                ? EventLogEntryType.Error
                : eventLevel == EventLevel.Warning
                    ? EventLogEntryType.Warning
                    : EventLogEntryType.Information;
        }
    }
}