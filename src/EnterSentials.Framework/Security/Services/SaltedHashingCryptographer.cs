using System;
using System.Security.Cryptography;
using System.Text;

namespace EnterSentials.Framework
{
    // Borrowed or adapted from: http://www.programminginterviews.info/2012/05/how-to-store-user-passwords-using.html
    public class SaltedHashingCryptographer : ICryptographer
    {
        private int GetTrueRandomNumber()
        {
            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // return a random number between 8 and 24
            return random.Next(8, 24);
        }


        private byte[] GetSaltBytes(string input, int saltSize)
        {
            byte[] saltBytes = null;
            // get some random bytes
            using (Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(input, saltSize, 1000))
            {
                // get salt
                saltBytes = rdb.Salt;
            }
            return saltBytes;
        }


        private string Encrypt(string input, byte[] saltBytes)
        {
            // get input bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // create salt + input array
            byte[] saltAndInput = new byte[saltBytes.Length + inputBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, saltAndInput, 0, saltBytes.Length);
            Buffer.BlockCopy(inputBytes, 0, saltAndInput, saltBytes.Length, inputBytes.Length);

            // hash the salt and input
            byte[] data = GetHashFor(saltAndInput);

            // append the salt length and raw salt to hashed data
            byte ss = Convert.ToByte(saltBytes.Length);

            byte[] finalBytes = new byte[1 + saltBytes.Length + data.Length];
            finalBytes[0] = ss;
            Buffer.BlockCopy(saltBytes, 0, finalBytes, 1, saltBytes.Length);
            Buffer.BlockCopy(data, 0, finalBytes, saltBytes.Length + 1, data.Length);

            // convert to base64 string
            string hash = ByteArrayToString(finalBytes);
            return hash;
        }


        private byte[] GetHashFor(byte[] input)
        {
            using (HashAlgorithm ha = new SHA512CryptoServiceProvider())
            {
                // copy into the return byte array
                byte[] data = new byte[input.Length];
                Array.Copy(input, data, input.Length);

                // process this atleast 1000 times
                for (int i = 0; i < 1000; i++)
                {
                    // Convert the input string to a byte array and compute the hash.
                    data = ha.ComputeHash(data);
                }
                return data;
            }
        }


        /// <summary>
        /// converts a byte array to a base 64 encoded string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ByteArrayToString(byte[] data)
        {
            string output = Convert.ToBase64String(data);
            return output;
        }


        /// <summary>
        /// converts a base 64 encoded string to a byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] StringToByteArray(string data)
        {
            byte[] output = Convert.FromBase64String(data);
            return output;
        }


        public bool Verify(string plainTextToVerify, string encryptedTextToMatch)
        {
            // convert encrypted text to bytes
            byte[] finalBytes = StringToByteArray(encryptedTextToMatch);

            // get salt size
            int saltSize = Convert.ToInt32(finalBytes[0]);

            // now get raw salt 
            byte[] saltBytes = new byte[saltSize];
            Array.Copy(finalBytes, 1, saltBytes, 0, saltSize);

            // using this recovered salt
            // generate a crypto hash of the input string
            string hash = Encrypt(plainTextToVerify, saltBytes);

            // check for match
            bool match = (String.Compare(encryptedTextToMatch, hash, false) == 0);

            return match;
        }


        public string GetEncrypted(string plainText)
        {
            // get random salt size
            int saltSize = GetTrueRandomNumber();

            // get random salt bytes
            byte[] saltBytes = GetSaltBytes(plainText, saltSize);

            // using this totally random and variable length salt
            // generate a crypto hash of the input string
            string hash = Encrypt(plainText, saltBytes);

            return hash;
        }
    }
}
