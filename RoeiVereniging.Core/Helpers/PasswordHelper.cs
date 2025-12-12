using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Helpers
{
    public static class PasswordHelper
    {
        // maybe move this to .env?
        private const int saltSize = 10;

        // enncrypt the password with bcrypt and returns the hashed password
        public static string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            // encrypt the password with bcrypt
            string passwordHashed = BCrypt.Net.BCrypt.HashPassword(password, saltSize);

            return passwordHashed;
        }

        // verify if the given password is the same as the encrypted password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
