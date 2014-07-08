namespace EnterSentials.Framework
{
    public class Claim
    {
        public static readonly string DefaultValueType = System.Security.Claims.ClaimValueTypes.String;
        public static readonly string DefaultIssuer = System.Security.Claims.ClaimsIdentity.DefaultIssuer;


        [RequiredAndNonDefaultNorEmpty]
        public string Type { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string Value { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string ValueType { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string Issuer { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string OriginalIssuer { get; set; }


        public System.Security.Claims.Claim ToDotNetClaim()
        { return new System.Security.Claims.Claim(Type, Value, ValueType, Issuer, OriginalIssuer); }


        public Claim()
        { }

        public Claim(string type, string value, string valueType = null, string issuer = null, string originalIssuer = null)
        {
            Guard.AgainstNullOrEmpty(type, "type");
            Guard.AgainstNullOrEmpty(value, "value");

            Type = type;
            Value = value;
            ValueType = valueType ?? DefaultValueType;
            Issuer = issuer ?? DefaultIssuer;
            OriginalIssuer = originalIssuer ?? Issuer;
        }
    }
}
