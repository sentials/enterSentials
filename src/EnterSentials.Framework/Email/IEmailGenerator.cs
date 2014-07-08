namespace EnterSentials.Framework
{
    public interface IEmailGenerator
    {
        Email GenerateEmail(string templateKey, dynamic templateParameters);
    }
}