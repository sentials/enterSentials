using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EnterSentials.Framework
{
    public static class StringExtensions
    {
        public static bool IsValidSsn(this string @string, out string ssn, bool allowJustLast4 = false)
        { return new SsnParser().TryParse(@string, out ssn, allowJustLast4); }

        public static bool IsValidId(this string @string)
        { return !string.IsNullOrEmpty(@string); }


        public static string UrlEncoded(this string @string)
        { return HttpUtility.UrlEncode(@string); }


        public static T DeserializeJsonTo<T>(this string @string)
        { return Components.Instance.Get<IJsonSerializer>().Deserialize<T>(@string); }

        public static object DeserializeJsonTo(this string @string, Type targetType)
        { return Components.Instance.Get<IJsonSerializer>().Deserialize(@string, targetType); }
        

        public static T DeserializeXmlTo<T>(this string @string)
        { return Components.Instance.Get<IXmlSerializer>().Deserialize<T>(@string); }

        public static object DeserializeXmlTo(this string @string, Type targetType)
        { return Components.Instance.Get<IXmlSerializer>().Deserialize(@string, targetType); }


        public static bool IsAssemblyQualifiedTypeName(this string @string, out string typeName, out string assemblyName)
        {
            typeName = null;
            assemblyName = null;
            var components = @string.Split(',');
            var hasCorrectFormat = components.Count() == 2;
            if (hasCorrectFormat)
            {
                typeName = components.First().Trim();
                assemblyName = components.Last().Trim();
            }
            return hasCorrectFormat;
        }

        public static bool IsAssemblyQualifiedTypeName(this string @string)
        {
            var ignored0 = (string) null;
            var ignored1 = (string) null;
            return @string.IsAssemblyQualifiedTypeName(out ignored0, out ignored1);
        }


        public static bool TryExtractGuid(this string @string, out Guid guid)
        {
            var matchedOrNot = false;
            guid = Guid.Empty;
            var match = RegularExpression.Guid.Match(@string);
            if (matchedOrNot = match.Success)
                guid = new Guid(match.Value);
            return matchedOrNot;
        }


        public static bool TryParseEmailAddress(this string @string, out MailAddress emailAddress)
        {
            emailAddress = null;

            try
            { emailAddress = new MailAddress(@string); }
            catch
            { }

            return emailAddress != null;
        }


        public static bool TryParseUserName(this string @string, out string userName)
        {
            userName = null;

            var emailAddress = (MailAddress) null;
            if (@string.TryParseEmailAddress(out emailAddress))
                userName = emailAddress.User;

            return !string.IsNullOrEmpty(userName);
        }


        public static bool TryParseHostName(this string @string, out string hostName)
        {
            hostName = null;

            var emailAddress = (MailAddress)null;
            if (@string.TryParseEmailAddress(out emailAddress))
                hostName = emailAddress.User;

            return !string.IsNullOrEmpty(hostName);
        }


        public static bool IsValidEmail(this string @string)
        { return new EmailAddressValidator().IsValidEmail(@string); }
    }
}