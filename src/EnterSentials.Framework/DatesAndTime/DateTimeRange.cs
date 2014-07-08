using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EnterSentials.Framework
{
    [ComplexType]
    public class DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool IsValid
        { get { return Start <= End; } }

        public bool IsADay
        { get { return Start.Date == End.Date; } }

        public bool IsMoment
        { get { return Start == End; } }

        public int DurationInDays
        { get { return (End - Start).Days; } }

        public TimeSpan Duration
        { get { return End - Start; } }


        public static bool Equals(DateTimeRange x, DateTimeRange y)
        { return x.Start == y.Start && x.End == y.End; }
    }
}