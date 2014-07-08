using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterSentials.Framework
{
    public static class DateTimeExtensions
    {
        private const int NumDaysInWeek = 7;

        // TODO: BiWeekly, Twice-Monthly, Quarterly, and Semi-Annually probably do not have the right implementation
        private static int GetOffsetDay(DateTime dateTime, BusinessPeriodType periodType)
        {
            return periodType == BusinessPeriodType.Weekly
                ? (int)dateTime.DayOfWeek
                : periodType == BusinessPeriodType.Daily
                    ? 0
                    : periodType == BusinessPeriodType.Biweekly
                        ? dateTime.DayOfYear % (NumDaysInWeek * 2)
                        : periodType == BusinessPeriodType.TwiceMonthly
                            ? dateTime.Day % 16
                            : periodType == BusinessPeriodType.Monthly
                                ? dateTime.Day
                                : periodType == BusinessPeriodType.Quarterly
                                    ? dateTime.DayOfYear % 92
                                    : periodType == BusinessPeriodType.SemiAnually
                                        ? dateTime.DayOfYear % 183
                                        : periodType == BusinessPeriodType.Annually
                                            ? dateTime.DayOfYear
                                            : 0;
        }


        public static bool HasPeriodEndOffsetDay(this DateTime dateTime, BusinessPeriodType periodType, int offsetDay)
        { return GetOffsetDay(dateTime, periodType) == offsetDay; }
    }
}
