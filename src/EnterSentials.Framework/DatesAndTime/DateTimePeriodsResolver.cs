using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterSentials.Framework
{
    internal class DateTimePeriodsResolver
    {
        private const int NumDaysInWeek = 7;

        private readonly DateTimeRange dateTimeRange = null;
        private readonly DateTime? dateTimeRangeFloor = null;
        private readonly DateTime? dateTimeRangeCeiling = null;
        private readonly BusinessPeriodType periodType = default(BusinessPeriodType);
        private readonly int periodEndOffsetDay = default(int);
        private readonly int periodEndOffsetHours = default(int);

        public IEnumerable<DateTimeRange> Periods { get; private set; }
        public IEnumerable<DateTimeRange> ContainingPeriods { get; private set; }


        private static DateTime GetContainingPeriodStartDateTimeFor(DateTime dateTime, BusinessPeriodType periodType, int offsetDay, int offsetHours)
        {
            var start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0,DateTimeKind.Utc);

            while (start > dateTime)
                start = start.AddDays(-1);

            if (offsetHours > 0)
            {
                while (!start.HasPeriodEndOffsetDay(periodType, offsetDay))
                    start = start.AddDays(-1);
            }
            else
            {
                while (!(start.AddDays(-1)).HasPeriodEndOffsetDay(periodType, offsetDay))
                    start = start.AddDays(-1);
            }

            return start;
        }


        private static DateTime GetFirstPeriodEndDateTimeFor(DateTime dateTime, BusinessPeriodType periodType, int offsetDay, int offsetHours)
        {
            var start = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, offsetHours, 0, 0);

            while (start > dateTime)
                start = start.AddDays(-1);

            if (offsetHours > 0)
            {
                while (!start.HasPeriodEndOffsetDay(periodType, offsetDay))
                    start = start.AddDays(-1);
            }
            else
            {
                while (!(start.AddDays(-1)).HasPeriodEndOffsetDay(periodType, offsetDay))
                    start = start.AddDays(-1);
            }

            return start;
        }

                
        private void ParseWeeklyPeriods()
        {           
            var containingPeriodsStartDateTime = GetContainingPeriodStartDateTimeFor(dateTimeRange.Start, periodType, periodEndOffsetDay, periodEndOffsetHours);


            var containingPeriod = new DateTimeRange
            {
                Start = new DateTime(containingPeriodsStartDateTime.Year, containingPeriodsStartDateTime.Month,
                                   containingPeriodsStartDateTime.Day, periodEndOffsetHours, 0, 0, DateTimeKind.Utc),
                End = new DateTime(containingPeriodsStartDateTime.Year,containingPeriodsStartDateTime.Month,
                                   containingPeriodsStartDateTime.Day,periodEndOffsetHours, 0, 0,DateTimeKind.Utc).AddDays(NumDaysInWeek - 1)
            };


            //var period = new DateTimeRange
            //{
            //    Start = dateTimeRange.Start,
            //    End = containingPeriod.End > dateTimeRange.End
            //        ? !dateTimeRangeCeiling.HasValue
            //            ? containingPeriod.End
            //            : (containingPeriod.End > dateTimeRangeCeiling.Value)
            //                ? dateTimeRangeCeiling.Value
            //                : containingPeriod.End
            //        : containingPeriod.End
            //};

            var period = new DateTimeRange
            {
                Start = (dateTimeRangeFloor.HasValue
                        && dateTimeRangeFloor.Value>containingPeriod.Start 
                        && dateTimeRangeFloor.Value<containingPeriod.End)
                        ?dateTimeRangeFloor.Value
                        :containingPeriod.Start,

                End = (dateTimeRangeCeiling.HasValue
                        && dateTimeRangeCeiling.Value < containingPeriod.End
                        && dateTimeRangeCeiling.Value > containingPeriod.Start)
                        ? dateTimeRangeCeiling.Value
                        : containingPeriod.End,
            };


            var GetNextContainingPeriod = new Func<DateTimeRange>(() =>
                new DateTimeRange
                {
                    Start = containingPeriod.Start.AddDays(NumDaysInWeek),
                    End = containingPeriod.End.AddDays(NumDaysInWeek)
                });

            //var GetNextPeriod = new Func<DateTimeRange>(() =>
            //{
            //    return new DateTimeRange
            //    {
            //        Start = containingPeriod.Start,
            //        End = containingPeriod.End > dateTimeRange.End ? dateTimeRange.End : containingPeriod.End
            //    };
            //});


            var GetNextPeriod = new Func<DateTimeRange>(() =>
            {
                return new DateTimeRange
                {
                    Start = containingPeriod.Start,
                    End = containingPeriod.End > dateTimeRange.End ? dateTimeRange.End : containingPeriod.End
                };
            });
            

            var containingPeriods = new Collection<DateTimeRange> { containingPeriod };
            ContainingPeriods = containingPeriods;

            var periods = new Collection<DateTimeRange> { period };

            while (dateTimeRange.End > containingPeriod.End)
            {
                containingPeriods.Add(containingPeriod = GetNextContainingPeriod());
                periods.Add(period = GetNextPeriod());
            }

            ContainingPeriods = containingPeriods;
            Periods = periods;

            //EPW - 9/12/2013 - commented out code below - Bug Fix: duplicate containing periods being returned
            //containingPeriods.Add(containingPeriod);
        }


        private void Parse()
        {
            var parse = (periodType == BusinessPeriodType.Weekly)
                ? ParseWeeklyPeriods
                : (Action)null;

            if (parse != null)
                parse();
        }


        private void Validate(BusinessPeriodType periodType, int periodEndOffsetDay)
        {
            if (periodType == BusinessPeriodType.Annually
                || periodType == BusinessPeriodType.SemiAnually
                || periodType == BusinessPeriodType.Quarterly
                || periodType == BusinessPeriodType.Monthly
                || periodType == BusinessPeriodType.TwiceMonthly
                || periodType == BusinessPeriodType.Biweekly
                || periodType == BusinessPeriodType.Daily)
                throw new NotImplementedException();

            if (periodType == BusinessPeriodType.Weekly)
                Guard.AgainstOutOfRange(periodEndOffsetDay, (int)DayOfWeek.Sunday, (int)DayOfWeek.Saturday, "periodEndOffsetDay");
        }


        public DateTimePeriodsResolver(
            DateTimeRange dateTimeRangeToParse,
            DateTime? dateTimeRangeCeiling,
            BusinessPeriodType periodType, 
            int periodEndOffsetDay,            
            int periodEndOffsetHours = 0
        )
        {
            Guard.Against(dateTimeRangeToParse, d => d == null || !d.IsValid, "Must be a valid date range.", "dateRangeToParse");
            Guard.AgainstOutOfRange(periodEndOffsetHours, "periodEndOffsetHours");
            Validate(periodType, periodEndOffsetDay);

            this.dateTimeRange = dateTimeRangeToParse;
            this.dateTimeRangeCeiling = dateTimeRangeCeiling;
            this.periodType = periodType;
            this.periodEndOffsetDay = periodEndOffsetDay;
            this.periodEndOffsetHours = periodEndOffsetHours;

            Parse();
        }

        public DateTimePeriodsResolver(
           DateTimeRange dateTimeRangeToParse,
           DateTime? dateTimeRangeFloor,
           DateTime? dateTimeRangeCeiling,
           BusinessPeriodType periodType,
           int periodEndOffsetDay,
           int periodEndOffsetHours = 0
       )
        {
            Guard.Against(dateTimeRangeToParse, d => d == null || !d.IsValid, "Must be a valid date range.", "dateRangeToParse");
            Guard.AgainstOutOfRange(periodEndOffsetHours, "periodEndOffsetHours");
            Guard.Against(dateTimeRangeFloor!=null && dateTimeRangeCeiling!=null && dateTimeRangeFloor>dateTimeRangeCeiling,"Invalid ceiling and floor values");
            Validate(periodType, periodEndOffsetDay);

            this.dateTimeRange = dateTimeRangeToParse;
            this.dateTimeRangeCeiling = dateTimeRangeCeiling;
            this.dateTimeRangeFloor = dateTimeRangeFloor;
            this.periodType = periodType;
            this.periodEndOffsetDay = periodEndOffsetDay;
            this.periodEndOffsetHours = periodEndOffsetHours;

            Parse();
        }
    }
}