using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LyricEditor
{
    public class DESCryptoHelper
    {
        private DESCryptoServiceProvider desCryptoServiceProvider;

        public DESCryptoHelper()
        {
            desCryptoServiceProvider = new DESCryptoServiceProvider();
        }

        public string Encrypt(string input, byte[] key)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);

            writer.Write(input);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public string Decrypt(string input, byte[] key)
        {
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(input));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(key, key), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }
    }
}
