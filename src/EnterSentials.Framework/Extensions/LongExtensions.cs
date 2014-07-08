namespace EnterSentials.Framework
{
    public static class LongExtensions
    {
        public static bool IsValidId(this long @long)
        { return @long > 0; }
    }
}