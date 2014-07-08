using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterSentials.Framework
{
    public static class DateTimeRangeExtensions
    {
        public static bool TryResolvePeriods(
            this DateTimeRange dateTimeRange,
            DateTime? dateTimeRangeCeiling,
            BusinessPeriodType periodType,
            int periodEndOffsetDay,
            int periodEndOffsetHours,
            out IEnumerable<DateTimeRange> containingPeriods,
            out IEnumerable<DateTimeRange> periods
        )
        {
            Guard.AgainstNull(dateTimeRange, "dateTimeRange");
            Guard.Against(dateTimeRange, d => !d.IsValid, "Must provide a valid DateTimeRange.", "dateTimeRange");

            var couldResolveOrNot = false;
            containingPeriods = null;
            periods = null;
            try
            {
                var resolver = new DateTimePeriodsResolver(dateTimeRange, dateTimeRangeCeiling, periodType, periodEndOffsetDay, periodEndOffsetHours);
                containingPeriods = resolver.ContainingPeriods;
                periods = resolver.Periods;
                couldResolveOrNot = true;
            }
            catch
            { }

            return couldResolveOrNot;
        }

        public static bool TryResolvePeriods(
            this DateTimeRange dateTimeRange,
            DateTime? dateTimeRangeCeiling,
            DateTime? dateTimeRangeFloor,
            BusinessPeriodType periodType,
            int periodEndOffsetDay,
            int periodEndOffsetHours,
            out IEnumerable<DateTimeRange> containingPeriods,
            out IEnumerable<DateTimeRange> periods
        )
        {
            Guard.AgainstNull(dateTimeRange, "dateTimeRange");
            Guard.Against(dateTimeRange, d => !d.IsValid, "Must provide a valid DateTimeRange.", "dateTimeRange");

            var couldResolveOrNot = false;
            containingPeriods = null;
            periods = null;
            try
            {
                var resolver = new DateTimePeriodsResolver(dateTimeRange, dateTimeRangeFloor, dateTimeRangeCeiling, periodType, periodEndOffsetDay, periodEndOffsetHours);
                containingPeriods = resolver.ContainingPeriods;
                periods = resolver.Periods;
                couldResolveOrNot = true;
            }
            catch
            { }

            return couldResolveOrNot;
        }


        public static IEnumerable<DateTimeRange> GetContainedDays(this DateTimeRange dateTimeRange)
        {
            Guard.AgainstNull(dateTimeRange, "dateTimeRange");

            var days = new Collection<DateTimeRange>();
            var day = new DateTimeRange { Start = dateTimeRange.Start, End = dateTimeRange.Start.AddDays(1) };

            days.Add(day);
            while (day.End <= dateTimeRange.End)
            {
                day = new DateTimeRange { Start = day.End, End = day.End.AddDays(1) };
                days.Add(day);
            }

            var lastDay = days.Last();
            if (lastDay.End > dateTimeRange.End)
                lastDay.End = dateTimeRange.End;

            return days;
        }


        public static bool Intersects(this DateTimeRange dateTimeRange, DateTime start, DateTime? end)
        {
            Guard.AgainstNull(dateTimeRange, "dateTimeRange");
            Guard.AgainstNull(start, "start");
            Guard.Against(dateTimeRange, d => !d.IsValid, "Provided DateTimeRange must be valid.", "dateTimeRange");

            return !end.HasValue
                ? dateTimeRange.End > start
                : ((dateTimeRange.Start <= start) && (dateTimeRange.End > start)) 
                    || ((dateTimeRange.Start < end) && (dateTimeRange.End >= end))
                    || ((dateTimeRange.Start >= start) && (dateTimeRange.End <= end))
                    || ((dateTimeRange.Start <= start) && (dateTimeRange.End >= end))
                    ;
        }

        public static bool Intersects(this DateTimeRange dateTimeRange, DateTimeRange otherDateTimeRange)
        { return dateTimeRange.Intersects(otherDateTimeRange.Start, otherDateTimeRange.End); }


        //public static IEnumerable<DateRange> PartitionIntoDays(this DateRange period)
        //{ throw new NotImplementedException(); }


        //public static IEnumerable<DateRange> PartitionIntoMonths(this DateRange period, int monthEndingDay)
        //{ throw new NotImplementedException(); }


        //public static IEnumerable<DateRange> PartitionIntoWeeks(this DateRange period, DayOfWeek weekEndingDay)
        //{
        //    var weeks = new Collection<DateRange>();
        //    var currentWeek = (DateRange)null;

        //    if (period.Start.DayOfWeek == weekEndingDay)
        //    {
        //        currentWeek = new DateRange { Start = period.Start.Date.AddDays(-6).Date, End = period.Start.Date };
        //    }
        //    else
        //    {
        //        var dayIntervalBetweenPeriodStartAndWeekEnd = Convert.ToInt32(weekEndingDay) - Convert.ToInt32(period.Start.DayOfWeek);
        //        var periodEnd = period.Start.Date.AddDays(dayIntervalBetweenPeriodStartAndWeekEnd).Date;
        //        var periodStart = periodEnd.Date.AddDays(-6).Date;
        //        currentWeek = new DateRange { Start = periodStart, End = periodEnd };
        //    }

        //    weeks.Add(currentWeek);

        //    while (currentWeek.End < period.End.Date)
        //    {
        //        currentWeek = new DateRange() { Start = currentWeek.End.Date.AddDays(1).Date, End = currentWeek.End.Date.AddDays(7).Date };
        //        weeks.Add(currentWeek);
        //    }

        //    return weeks;
        //}


        //public static bool IsBefore(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid && (other.End >= period.Start);

        //    return false;
        //}


        //public static bool IsBefore(this DateRange period, DateTime? date)
        //{
        //    if (period != null)
        //        return period.IsValid && (date == null || period.End < date);

        //    return false;
        //}


        //public static bool IsAfter(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid && (other.Start >= period.End);

        //    return false;
        //}


        //public static bool IsEqualTo(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid && (other.Start == period.Start && other.End == period.End);

        //    return false;
        //}


        //public static bool Intersects(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid &&
        //            (((other.Start >= period.Start && other.Start <= period.End) || (other.End <= period.End && other.End >= period.Start))) ||
        //            (((period.Start >= other.Start && period.Start <= other.End) || (period.End <= other.End && period.End >= other.Start)));

        //    return false;
        //}


        //public static bool Contains(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid && (other.Start >= period.Start && other.End <= period.End);

        //    return false;
        //}


        //public static bool IsContainedBy(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid && (other.Start <= period.Start && other.End >= period.End);

        //    return false;
        //}


        //public static bool IsBefore(this DateRange period, DateTime start, DateTime end)
        //{
        //    if (period != null)
        //        return period.IsValid && (start <= end) && (end >= period.Start);

        //    return false;
        //}


        //public static bool IsAfter(this DateRange period, DateTime start, DateTime end)
        //{
        //    if (period != null)
        //        return period.IsValid && (start <= end) && (start >= period.End);

        //    return false;
        //}


        //public static bool EndsAfter(this DateRange period, DateTime date)
        //{
        //    if (period != null)
        //        return period.IsValid && date > period.End;

        //    return false;
        //}


        //public static bool EndsBefore(this DateRange period, DateTime date)
        //{
        //    if (period != null)
        //        return period.IsValid && date <= period.End;

        //    return false;
        //}


        //public static bool EndsAfter(this DateRange period, DateTime? date)
        //{
        //    if (period != null)
        //        return period.IsValid && (date != null) && date > period.End;

        //    return false;
        //}


        //public static bool EndsBefore(this DateRange period, DateTime? date)
        //{
        //    if (period != null)
        //        return period.IsValid && (date != null) && date <= period.End;

        //    return false;
        //}


        //public static bool IsEqualTo(this DateRange period, DateTime start, DateTime end)
        //{
        //    if (period != null)
        //        return period.IsValid && (start <= end) && (start == period.Start && end == period.End);

        //    return false;
        //}

        //public static bool Intersects(this DateRange period, DateRange other)
        //{
        //    if (period != null && other != null)
        //        return period.IsValid && other.IsValid &&
        //            (((other.Start >= period.Start && other.Start <= period.End) || (other.End <= period.End && other.End >= period.Start))) ||
        //            (((period.Start >= other.Start && period.Start <= other.End) || (period.End <= other.End && period.End >= other.Start)));

        //    return false;
        //}
        //public static bool Intersects(this DateRange period, DateTime start, DateTime end)
        //{
        //    if (period != null)
        //        return period.IsValid && (start <= end) &&
        //            (((start >= period.Start && start <= period.End) || (end <= period.End && end >= period.Start))) ||
        //            (((period.Start >= start && period.Start <= end) || (period.End <= end && period.End >= start)));

        //    return false;
        //}


        //public static bool Intersects(this DateRange period, DateTime start, DateTime? end)
        //{
        //    if (period != null)
        //        return (end != null)
        //                   ? (period.IsValid && (start <= end) &&
        //                      (((start >= period.Start && start <= period.End) ||
        //                        (end <= period.End && end >= period.Start))) ||
        //                      (((period.Start >= start && period.Start <= end) ||
        //                        (period.End <= end && period.End >= start))))
        //                   : period.Start <= start && period.End >= start;

        //    return false;
        //}


        //public static bool Contains(this DateRange period, DateTime start, DateTime end)
        //{
        //    if (period != null)
        //        return period.IsValid && (start <= end) && (start >= period.Start && end <= period.End);

        //    return false;
        //}


        //public static bool IsContainedBy(this DateRange period, DateTime start, DateTime end)
        //{
        //    if (period != null)
        //        return period.IsValid && (start <= end) && (start <= period.Start && end >= period.End);

        //    return false;
        //}
    }
}
