using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagmentSystem.Domain.Core.Helpers
{
    public static class PasswordGenerator
    {
        private static readonly Random Random = new Random();
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=<>?";

        public static string GeneratePassword(int length = 12)
        {
            var password = new char[length];
            for (int i = 0; i < length; i++)
            {
                password[i] = Characters[Random.Next(Characters.Length)];
            }
            return new string(password);
        }
    }
}
