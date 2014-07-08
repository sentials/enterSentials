using EnterSentials.Framework.Properties;
using System.Collections.Generic;

namespace EnterSentials.Framework
{
    public static class Cors
    {
        public static readonly string DefaultAllowedOrigin = Http.Header.AccessControl.Allow.AnyOrigin;
        public static readonly string DefaultAllowedMethods = Http.Combine(Http.Methods.MetadataRetrieving, Http.Methods.Safe, Http.Methods.Unsafe);
        public static readonly string DefaultAllowedHeaders = Http.Combine(Http.Header.XRequestedWith, Http.Header.ContentType, Http.Header.Accept, Http.Header.Authorization);
        public static readonly IEnumerable<string> DefaultAllowedOrigins = new string[] { DefaultAllowedOrigin };
        

        public static readonly bool ShouldIncludeNoCacheHeader = Settings.Default.ShouldIncludeNoCacheHeaderForCors;
        public static readonly bool ShouldIncludePreflightResponseMaxAgeHeader = Settings.Default.ShouldIncludePreflightResponseMaxAgeHeaderForCors;
        public static readonly int PreflightResponseMaxAgeInSeconds = Settings.Default.PreflightResponseMaxAgeInSeconds;
    }
}