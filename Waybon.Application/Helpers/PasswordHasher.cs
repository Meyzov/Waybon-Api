using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Waybon.Application.Helpers
{
    public static class PasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 32;
        private static readonly int Iterations = 100000;

        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2
            (
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
        }

        public static bool Verify(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = Convert.FromBase64String(parts[1]);

            var actualHash = Rfc2898DeriveBytes.Pbkdf2
            (
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
