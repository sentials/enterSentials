using System.Linq;

namespace EnterSentials.Framework
{
    public class EmailTemplate
    {
        public const string ParameterFormat = "{{{{{0}}}}}";

        public static readonly EmailTemplate Default = new EmailTemplate
        {
            SubjectTemplate = GetParameterTemplate(Name.Of.Property.On<Email>(e => e.Subject)),
            BodyTemplate = GetParameterTemplate(Name.Of.Property.On<Email>(e => e.Body)),
        };


        public string SubjectTemplate { get; set; }
        public string BodyTemplate { get; set; }


        private static string Sanitized(object parameterValue)
        { return parameterValue == null ? "" : parameterValue.ToString(); }


        private static string GetContent(string template, object parameters)
        {
            Guard.AgainstNullOrEmpty(template, "template");
            var content = template;

            if (parameters != null)
            {
                var properties = parameters.GetType().GetProperties().Where(p => p.GetGetMethod() != null).ToArray();
                foreach (var property in properties)
                    content = content.Replace(
                        GetParameterTemplate(property.Name),
                        Sanitized(property.GetValue(parameters)));
            }

            return content;
        }


        public string GetSubjectUsing(object parameters)
        { return GetContent(SubjectTemplate ?? Default.SubjectTemplate, parameters); }


        public string GetBodyUsing(object parameters)
        { return GetContent(BodyTemplate ?? Default.BodyTemplate, parameters); }


        public static string GetParameterTemplate(string parameterName)
        {
            Guard.AgainstNullOrEmpty(parameterName, "parameterName");
            return string.Format(ParameterFormat, parameterName);
        }
    }
}