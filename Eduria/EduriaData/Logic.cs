using System;
using System.Security.Cryptography;

namespace EduriaData
{
    public class Logic
    {
        const int SaltSize = 16, HashSize = 20, HashIter = 10000;
        readonly byte[] _salt, _hash;
        public byte[] Salt { get { return (byte[])_salt.Clone(); } }
        public byte[] Hash { get { return (byte[])_hash.Clone(); } }

        /// <summary>
        /// Creates logic class with given string.
        /// </summary>
        /// <param name="password"></param>
        public Logic(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }
        /// <summary>
        /// Creates logic class with hashbytes.
        /// </summary>
        /// <param name="hashBytes"></param>
        public Logic(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }
        /// <summary>
        /// Creates logic class with salt and hash.
        /// </summary>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        public Logic(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }
        /// <summary>
        /// Converts logic class to bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }
        /// <summary>
        /// Verifies if the string is identical to created hash.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Verify(string password)
        {
            byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
                if (test[i] != _hash[i])
                    return false;
            return true;
        }
    }
}
