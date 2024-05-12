using System.Security.Cryptography;
using System.Text;

namespace colab_api.Services
{
    public class LoginService
    {
        private readonly byte[] key; // คีย์สำหรับ AES256


        public string EncryptAES256(string key, string plainText)
        {
            byte[] iV = new byte[16];
            byte[] inArray;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iV;
                ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(plainText);
                }

                inArray = memoryStream.ToArray();
            }

            return Convert.ToBase64String(inArray);
        }

        public  string DecryptAES256(string key, string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return string.Empty;
            }

            try
            {
                byte[] iV = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);
                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iV;
                ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
                using MemoryStream stream = new MemoryStream(buffer);
                using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(stream2);
                return streamReader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
