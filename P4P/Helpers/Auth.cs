using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace P4P.Helpers
{
    public static class Auth
    {
        public static string Getlogintoken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "").Replace("+", "");
        }

        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        public static bool VerifyHash(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static bool IsAuth()
        {
            return HttpContext.Current.Session["Id"] == null;
        }
    }
}