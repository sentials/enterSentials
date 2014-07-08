namespace EnterSentials.Framework
{
    public class CryptographerBasedSsnManager : ISsnManager
    {
        private readonly ICryptographer cryptographer = null;


        public bool Verify(string plainTextToVerify, string encryptedTextToMatch)
        { return cryptographer.Verify(plainTextToVerify, encryptedTextToMatch); }

        public string GetEncrypted(string plainText)
        { return cryptographer.GetEncrypted(plainText); }


        public CryptographerBasedSsnManager(ICryptographer cryptographer)
        {
            Guard.AgainstNull(cryptographer, "cryptographer");
            this.cryptographer = cryptographer;
        }
    }
}
