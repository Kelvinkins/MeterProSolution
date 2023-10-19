using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MeterPro.DATA.Utils
{
    public class Cryptography
    {

        public static string Encrypt(string plainText, string password, string offset)
        {
            try
            {


                // Convert the password and offset to bytes
                byte[] keyBytes = Encoding.UTF8.GetBytes(password);
                byte[] ivBytes = Encoding.UTF8.GetBytes(offset);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.None;
                    aesAlg.Key = keyBytes;
                    aesAlg.IV = ivBytes;

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(plainText);
                            }
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }catch(Exception ex)
            {
                string message = ex.Message;
                return "";
            }
        }

        public static string Decrypt(string cipherText, string password, string offset)
        {
            // Convert the password and offset to bytes
            byte[] keyBytes = Encoding.UTF8.GetBytes(password);
            byte[] ivBytes = Encoding.UTF8.GetBytes(offset);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.None;
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }


}
