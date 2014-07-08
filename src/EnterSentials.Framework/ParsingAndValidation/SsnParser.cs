using System.Linq;

namespace EnterSentials.Framework
{
    public class SsnParser
    {
        public bool TryParse(string ssn, out string ssnOrJustLast4, bool alsoTryParseJustLast4 = false)
        {
            ssnOrJustLast4 = null;

            if (!string.IsNullOrEmpty(ssn))
            {
                var isValid = (alsoTryParseJustLast4 && ssn.Length == 4)
                    ? ssn.ToCharArray().All(c => char.IsDigit(c))
                    : ((ssn.Length == 9) && (ssn.ToCharArray().All(c => char.IsDigit(c))))
                    || ((ssn.Length == 11)
                        && (ssn[3] == '-')
                        && (ssn[6] == '-')
                        && (ssn.ToCharArray().Except(new char[] { ssn[3], ssn[6] }).All(c => char.IsDigit(c))));

                if (isValid)
                    ssnOrJustLast4 = new string(ssn.ToCharArray().Where(char.IsDigit).ToArray());
            }

            return !string.IsNullOrEmpty(ssnOrJustLast4);
        }
    }
}
