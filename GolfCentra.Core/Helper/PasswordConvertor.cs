using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace GolfCentra.Core.Helper
{
    public static class PasswordConvertor
    {
        /// <summary>
        /// Convert Password To Md5 Type Password
        /// </summary>
        /// <param name="password"> Password Provided By User</param>
        /// <returns>Md5 Type Password</returns>
        public static string Password(string password)
        {
           var x= HashPassword(password);
            return x.Item1;
            var y = ValidatePassword(password, x.Item1);
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedBytes;

            //Create A UTF8Encoding Object We Will Use To Convert Our Password String To A Byte Array
            UTF8Encoding encoder = new UTF8Encoding();

            //Encrypt The Password And Store It In The HashedBytes Byte Array
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sBuilder.Append(hashedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }

        public static Tuple<string, string> HashPassword(string password)
        {
            //"$2a$12$mhRDl2FaTgNT0OeH7zOpy.
            string salt = GetRandomSalt();
            return new Tuple<string, string>(BCrypt.Net.BCrypt.HashPassword(password, salt), salt);
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Net.BCrypt.Verify(password , correctHash);
        }
    }
}
