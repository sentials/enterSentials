using System;
//
namespace EnterSentials.Framework
{
    // Borrowed and adapted from internal components in multiple open source frameworks (such as Unity)
    public class Guard
    {
        public static void ThrowInvalidOperationException(string message)
        { throw new InvalidOperationException(message); }

        public static void ThrowArgumentException(string message, string paramName)
        { throw new ArgumentException(message, paramName); }

        public static void ThrowArgumentNullException(string paramName)
        { throw new ArgumentNullException(paramName); }

        public static void ThrowStringArgumentIsNullOrEmptyException(string paramName)
        { ThrowArgumentException("cannot be null or empty", paramName); }

        public static void ThrowArgumentOutOfRangeException(string paramName)
        { throw new ArgumentOutOfRangeException(paramName); }


        public static void AgainstNull(object sut, string paramName)
        {
            if (sut == null)
                ThrowArgumentNullException(paramName);
        }

        public static void AgainstNullOrEmpty(string sut, string paramName)
        {
            if (string.IsNullOrEmpty(sut))
                ThrowStringArgumentIsNullOrEmptyException(paramName);
        }

        public static void AgainstOutOfRange(int indexOrCount, string paramName)
        {
            if (indexOrCount < 0)
                ThrowArgumentOutOfRangeException(paramName);
        }

        public static void AgainstOutOfRange(int value, int rangeMin, int rangeMax, string paramName)
        { AgainstOutOfRange((double)value, (double)rangeMin, (double)rangeMax, paramName); }

        public static void AgainstOutOfRange(double value, double minRange, double maxRange, string paramName)
        {
            if ((value < minRange) || (value > maxRange))
                ThrowArgumentOutOfRangeException(paramName);
        }

        public static void AgainstOutOfRange<T>(T value, T minRange, T maxRange, string paramName)
            where T : IComparable
        {
            if ((value.CompareTo(minRange) < 0) || (value.CompareTo(maxRange) > 0))
                ThrowArgumentOutOfRangeException(paramName);
        }


        public static void Against<TParam>(
            TParam param,
            Predicate<TParam> conditionIndicatingInvalidParam,
            string message,
            string paramName
        )
        {
            if (conditionIndicatingInvalidParam(param))
                ThrowArgumentException(message, paramName);
        }

        public static void Against(bool conditionIndicatingInvalidOperation, string message)
        {
            if (conditionIndicatingInvalidOperation)
                ThrowInvalidOperationException(message);
        }
    }
}
