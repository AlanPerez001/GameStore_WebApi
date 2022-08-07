using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GameStore_WebApi.Utility
{
    public class CryptoService
    {
        public static string Encrypt(string input)
        {
            string llave = Startup.respuestasApi.CryptoServiceKey;
            string value = null;
            try
            {
                RijndaelManaged AES = new RijndaelManaged();
                MD5CryptoServiceProvider Hash_AES = new MD5CryptoServiceProvider();
                byte[] hash = System.Text.Encoding.ASCII.GetBytes(llave);
                AES.Key = hash;
                AES.Mode = CipherMode.ECB;
                ICryptoTransform DESEncrypter = AES.CreateEncryptor();
                byte[] Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(input);
                Buffer = DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length);
                value = Convert.ToBase64String(Buffer);
            }
            catch (Exception ex)
            {
                value = null;
            }
            return value;
        }
    }
}
