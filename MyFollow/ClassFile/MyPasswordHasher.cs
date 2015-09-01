using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace MyFollow.ClassFile
{
    public class MyPasswordHasher 
    {
        //public static string MD5Hash(string value)
        //{
        //    return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(new UTF8Encoding().GetBytes(value)));
        //}
        public string HashPassword(string password)
        {
            //const string salt = "Cotopaxi";
            //byte[] bytes = Encoding.ASCII.GetBytes(salt);

            //var hmacMD5 = new HMACMD5(bytes);


            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            //var saltedHash = hmacMD5.ComputeHash(inputBytes);
            byte[] hash = md5.ComputeHash(inputBytes);


            //// step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}