using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GSXF.Security
{
    public class Encryption
    {
        public static string SHA256(string plainText)
        {
            SHA256Managed sha256 = new SHA256Managed();
            byte[] cipherText = sha256.ComputeHash(Encoding.Default.GetBytes(plainText));
            return System.Convert.ToBase64String(cipherText);
        }
    }
}
