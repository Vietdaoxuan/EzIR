using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CoreLib.Commons
{
    public static class PasswordGenerator
    {
        public static readonly string SPECIAL_CHARS = "~@#$%^()_[]{}";

        public static string GetOtpPass()
        {
            string otpPass = ThreadSafeRandom.Next(100000, 999999).ToString();

            return otpPass;
        }

        public static string GetRandomPassword()
        {
            var builder = new StringBuilder();

            //builder.Append(GetRandomSpecialString(2));
            //builder.Append(GetRandomString(2, false));
            //builder.Append(GetRandomString(2, true));

            builder.Append(GetRandomNumber(10, 99));
            builder.Append(GetRandomNumber(01, 99));
            builder.Append(GetRandomNumber(01, 99));

            return builder.ToString();
        }

        public static string GetRandomSpecialString(int size)
        {
            string chars = SPECIAL_CHARS;

            var randomString = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                int characterIndex = Convert.ToInt32(ThreadSafeRandom.Next(i, chars.Length - 1));

                randomString.Append(chars[characterIndex]);
            }

            return randomString.ToString();
        }

        private static int GetRandomNumber(int min, int max)
        {
            return ThreadSafeRandom.Next(min, max);
        }

        public static string GetRandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();

            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * ThreadSafeRandom.NextDouble() + 65)));
                builder.Append(ch);
            }

            if (lowerCase) return builder.ToString().ToLower();

            return builder.ToString();
        }

        public static string EncodePassword(string plaintext)
        {
            string secretCode = "IR@123";

            var encoding = new UTF8Encoding();

            byte[] input = encoding.GetBytes(plaintext + secretCode);

            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] output = md5.ComputeHash(input);

            string encrypted = BitConverter.ToString(output);

            string result = encrypted.Replace("-", "").ToLower();

            return result;
        }
    }

    internal class ThreadSafeRandom
    {
        private static readonly Random random = new Random();

        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        public static double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
