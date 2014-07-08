using System.Text.RegularExpressions;

namespace EnterSentials.Framework
{
    public static class RegularExpression
    {
        public static readonly Regex Guid = new Regex("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.Compiled);
    }
}
