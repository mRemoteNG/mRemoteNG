using System;
using System.Text;
using Org.BouncyCastle.Security;

namespace mRemoteNG.Security
{
    public class RandomGenerator
    {
        public static string RandomString(int length)
        {
            if (length < 0)
                throw new ArgumentException($"{nameof(length)} must be a positive integer");

            var randomGen = new SecureRandom();
            var stringBuilder = new StringBuilder();
            const string availableChars = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+|[]{};:',./<>?";
            for (var x = 0; x < length; x++)
            {
                var randomIndex = randomGen.Next(availableChars.Length - 1);
                stringBuilder.Append(availableChars[randomIndex]);
            }
            return stringBuilder.ToString();
        }
    }
}