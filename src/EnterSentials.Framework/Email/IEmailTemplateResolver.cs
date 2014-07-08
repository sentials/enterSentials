namespace EnterSentials.Framework
{
    public interface IEmailTemplateResolver
    {
        EmailTemplate GetEmailTemplate(string templateKey);
    }
}