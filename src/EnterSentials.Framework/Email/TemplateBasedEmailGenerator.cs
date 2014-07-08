namespace EnterSentials.Framework
{
    public class TemplateBasedEmailGenerator : IEmailGenerator
    {
        private readonly IEmailTemplateResolver templateResolver = null;


        public Email GenerateEmail(string templateKey, object templateParameters)
        {
            var template = templateResolver.GetEmailTemplate(templateKey);
            Guard.Against(template == null, "Must be able to resolve an email template for the provided key.");

            var email = new Email().CopyFrom(templateParameters.GetType(), templateParameters);

            email.Subject = template.GetSubjectUsing(templateParameters);
            email.Body = template.GetBodyUsing(templateParameters);

            return email;
        }


        public TemplateBasedEmailGenerator(IEmailTemplateResolver templateResolver)
        {
            Guard.AgainstNull(templateResolver, "templateResolver");
            this.templateResolver = templateResolver;
        }
    }
}
