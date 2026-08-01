using System.Security.Cryptography;

namespace Waybon.Application.Helpers
{
    public static class JoinCodeGenerator
    {
        private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static string Generate(int length = 6)
        {
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = AllowedChars[RandomNumberGenerator.GetInt32(AllowedChars.Length)];
            }
            return new string(result);
        }
    }
}