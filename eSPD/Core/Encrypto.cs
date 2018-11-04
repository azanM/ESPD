using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace eSPD.Core
{
    public class Encrypto
    {
        public static string Encrypt(string data)
        {
            using (var des = new TripleDESCryptoServiceProvider { Mode = CipherMode.ECB, Key = GetKey("a1!B78s!5(j$S1c%"), Padding = PaddingMode.PKCS7 })
            using (var desEncrypt = des.CreateEncryptor())
            {
                var buffer = Encoding.UTF8.GetBytes(data);

                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
        }

        public static string Decrypt(string data)
        {
            using (var des = new TripleDESCryptoServiceProvider { Mode = CipherMode.ECB, Key = GetKey("a1!B78s!5(j$S1c%"), Padding = PaddingMode.PKCS7 })
            using (var desDecrypt = des.CreateDecryptor())
            {
                var buffer = Convert.FromBase64String(data.Replace(" ", "+"));

                return Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
        }

        private static byte[] GetKey(string p)
        {
            string pwd = null;

            if (Encoding.UTF8.GetByteCount(p) < 24)
            {
                pwd = p.PadRight(24, ' ');
            }
            else
            {
                pwd = p.Substring(0, 24);
            }
            return Encoding.UTF8.GetBytes(pwd);
        }
    }
}
