using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LyricEditor
{
    public class RSACryptoHelper
    {
        private const int PROVIDER_RSA_FULL = 1;
        private const string CONTAINER_NAME = "ContainerName";
        private RSACryptoServiceProvider rsaCryptoServiceProvider;

        public RSACryptoHelper()
        {
            AssignParameter();
        }

        public string Encrypt(string input, string keyPath)
        {
            StreamReader streamReader = null;
            try
            {
                streamReader = new StreamReader(keyPath);
                string[] segmentedString = new string[input.Length / 100 + 1];
                for (int i = 0; i < segmentedString.Length; i++)
                    segmentedString[i] = input.Substring(i * 100, (((i + 1) * 100) < input.Length) ? 100 : input.Length - (i * 100));
                string publicOnlyKeyXML = streamReader.ReadToEnd();
                rsaCryptoServiceProvider.FromXmlString(publicOnlyKeyXML);

                string result = "";
                for (int i = 0; i < segmentedString.Length; i++)
                {
                    byte[] plainbytes = Encoding.UTF8.GetBytes(segmentedString[i]);
                    byte[] cipherbytes = rsaCryptoServiceProvider.Encrypt(plainbytes, false);
                    result += Convert.ToBase64String(cipherbytes);
                }
                return result;
            }
            finally
            {
                streamReader.Close();
            }
        }

        public string Decrypt(string input, string keyPath)
        {
            byte[] getpassword = null;
            StreamReader reader = new StreamReader(keyPath);
            try
            {
                getpassword = Convert.FromBase64String(input);
                string publicPrivateKeyXML = reader.ReadToEnd();
                rsaCryptoServiceProvider.FromXmlString(publicPrivateKeyXML);

                byte[] plain = rsaCryptoServiceProvider.Decrypt(getpassword, false);
                return Encoding.UTF8.GetString(plain);
            }
            finally
            {
                reader.Close();
            }
        }

        public void CreateXMLKey(string key, string path, bool includePrivateParam)
        {
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(path);
                string publicPrivateKeyXML = rsaCryptoServiceProvider.ToXmlString(includePrivateParam);
                streamWriter.Write(publicPrivateKeyXML);
            }
            finally
            {
                streamWriter.Close();
            }
        }

        private void AssignParameter()
        {
            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            rsaCryptoServiceProvider = new RSACryptoServiceProvider(cspParams);
        }
    }
}
