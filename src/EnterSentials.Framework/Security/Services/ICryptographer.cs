namespace EnterSentials.Framework
{
    public interface ICryptographer
    {
        bool Verify(string plainTextToVerify, string encryptedTextToMatch);
        string GetEncrypted(string plainText);
    }
}