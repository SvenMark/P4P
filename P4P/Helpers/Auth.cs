using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace P4P.Helpers
{
    public static class Auth
    {
        public static string Getlogintoken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 13);
        }
    }
}