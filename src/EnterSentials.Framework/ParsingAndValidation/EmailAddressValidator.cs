using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://msdn.microsoft.com/en-us/library/01escwtf.aspx
    public class EmailAddressValidator
    {
        private bool invalid = false;


        private string HostMapper(Match match)
        {
            // IdnMapping class with default property values.
            var idn = new IdnMapping();
            var hostName = match.Groups[2].Value;

            try
            { hostName = idn.GetAscii(hostName); }
            catch (ArgumentException)
            { invalid = true; }

            return match.Groups[1].Value + hostName;
        }


        public bool IsValidEmail(string strIn)
        {
            invalid = false;

            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(
                    strIn, 
                    @"(@)(.+)$", 
                    HostMapper,
                    RegexOptions.None, 
                    TimeSpan.FromMilliseconds(200)
                );
            }
            catch (RegexMatchTimeoutException)
            { return false; }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            { return false; }
        }
    }
}
