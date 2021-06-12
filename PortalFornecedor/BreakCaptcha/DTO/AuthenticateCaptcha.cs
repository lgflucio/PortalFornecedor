using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakCaptcha.DTO
{
    public static class AuthenticateCaptcha
    {
        public static User Authenticate(string username, string password)
        {
            var _users = new List<User>();
            _users.Add(new User { Username = "gswusr", Password = "fggkOu46H7hs#4tgfd@xcvwo", Role = "gswCaptcha" });
            return _users.Where(wh => wh.Username.ToLower() == username.ToLower() && wh.Password == password).FirstOrDefault();
        }
    }
}
