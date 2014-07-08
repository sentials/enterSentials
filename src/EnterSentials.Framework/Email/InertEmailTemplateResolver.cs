namespace EnterSentials.Framework
{
    public class InertEmailTemplateResolver : IEmailTemplateResolver
    {
        public EmailTemplate GetEmailTemplate(string templateKey)
        { return EmailTemplate.Default; }
    }
}