using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TopLearn.Core.Security
{
    public static class PasswordHelper
    {
        public static string EncodePasswordMD5(string password)
        {
            Byte[] OrginalBytes;
            Byte[] encodedBytes;

            MD5 md5;

            md5 = new MD5CryptoServiceProvider();
            
            OrginalBytes = ASCIIEncoding.Default.GetBytes(password);
            encodedBytes = md5.ComputeHash(OrginalBytes);

            return BitConverter.ToString(encodedBytes);
        }
    }
}
