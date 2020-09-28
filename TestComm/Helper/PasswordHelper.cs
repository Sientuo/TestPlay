using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestComm.Helper
{
    public class PasswordHelper
    {
        public static string GetSha1(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }
    }
}
