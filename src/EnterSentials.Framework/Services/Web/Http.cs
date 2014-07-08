using System.Collections.Generic;
using System.Linq;

namespace EnterSentials.Framework
{
    public static class Http
    {
        public static string Combine(IEnumerable<string> strings)
        { return string.Join(",", strings.Distinct()); }

        public static string Combine(params string[] strings)
        { return Combine(strings.AsEnumerable()); }

        public static string Combine(IEnumerable<IEnumerable<string>> groups)
        { return Combine(groups.Aggregate((g1, g2) => g1.Concat(g2))); }

        public static string Combine(params IEnumerable<string>[] groups)
        { return Combine(groups.AsEnumerable()); }


        public static class Method
        {
            public const string Connect = "CONNECT";
            public const string Trace = "TRACE";
            public const string Options = "OPTIONS";
            public const string Head = "HEAD";
            public const string Get = "GET";
            public const string Post = "POST";
            public const string Put = "PUT";
            public const string Delete = "DELETE";
            public const string Patch = "PATCH";
        }

        public static class Methods
        {
            public static readonly IEnumerable<string> Diagnostic = new string[] { Http.Method.Trace };
            public static readonly IEnumerable<string> MetadataRetrieving = new string[] { Http.Method.Head, Http.Method.Options };
            public static readonly IEnumerable<string> Safe = new string[] { Http.Method.Head, Http.Method.Get, Http.Method.Trace, Http.Method.Options };
            public static readonly IEnumerable<string> Unsafe = new string[] { Http.Method.Post, Http.Method.Put, Http.Method.Delete, Http.Method.Patch };
            public static readonly IEnumerable<string> Idempotent = new string[] { Http.Method.Trace, Http.Method.Options, Http.Method.Head, Http.Method.Get, Http.Method.Put, Http.Method.Delete };
            public static readonly IEnumerable<string> ProxyRelated = new string[] { Http.Method.Connect };
            public static readonly IEnumerable<string> Simple = new string[] { Http.Method.Head, Http.Method.Get };
            public static readonly IEnumerable<string> Complex = new string[] { Http.Method.Put, Http.Method.Delete };
            public static readonly IEnumerable<string> All = new string[] { 
                Http.Method.Connect, Http.Method.Trace, Http.Method.Options, Http.Method.Head, Http.Method.Get, Http.Method.Post, Http.Method.Put, Http.Method.Delete, Http.Method.Patch };
        }


        public static class Header
        {
            public static class AccessControl
            {
                public static class Request
                {
                    public const string Method = "Access-Control-Request-Method";
                    public const string Headers = "Access-Control-Request-Headers";
                }

                public static class Allow
                {
                    public const string AnyOrigin = "*";
                    public const string Origin = "Access-Control-Allow-Origin";
                    public const string Headers = "Access-Control-Allow-Headers";
                    public const string Methods = "Access-Control-Allow-Methods";
                    public const string Credentials = "Access-Control-Allow-Credentials";
                }

                public static class Expose
                {
                    public const string Headers = "Access-Control-Expose-Headers";
                }


                public const string MaxAge = "Access-Control-Max-Age";
            }


            public const string Accept = "Accept";
            public const string AcceptLanguage = "Accept-Language";
            public const string Authorization = "Authorization";
            public const string CacheControl = "Cache-Control"; 
            public const string ContentLanguage = "Content-Language";
            public const string ContentType = "Content-Type";
            public const string Expires = "Expires";
            public const string LastModified = "Last-Modified";
            public const string NoCache = "no-cache";
            public const string Origin = "Origin";
            public const string Pragma = "Pragma";
            public const string XRequestedWith = "X-Requested-With";
        }


        public static class Headers
        {
            public static class Request
            {
                public static readonly IEnumerable<string> Simple = new string[] {
                    Header.Origin, Header.Accept, Header.AcceptLanguage, Header.ContentLanguage};
            }

            public static class Response
            {
                public static readonly IEnumerable<string> Simple = new string[] {
                    Header.CacheControl, Header.ContentLanguage, Header.ContentType, Header.Expires, Header.LastModified, Header.Pragma };
            }
        }
    }
}