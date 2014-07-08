using System.ComponentModel.DataAnnotations;

namespace EnterSentials.Framework
{
    public class Email
    {
        [EmailAddress]
        [RequiredAndNonDefaultNorEmpty]
        public string ToAddress { get; set; }

        [EmailAddress]
        [RequiredAndNonDefaultNorEmpty]
        public string FromAddress { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string FromDisplayName { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string Subject { get; set; }

        [RequiredAndNonDefaultNorEmpty]
        public string Body { get; set; }
    }
}