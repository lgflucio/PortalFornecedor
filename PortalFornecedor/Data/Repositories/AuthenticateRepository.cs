using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repositories
{
    public static class AuthenticateRepository
    {
        public static AuthenticateDTO Get(string login, string password)
        {
            List<AuthenticateDTO> users = new List<AuthenticateDTO>();
            users.Add(new AuthenticateDTO { Id = 1, Login = "lgferreira", Password = "12345678", Role = "admin" });
            users.Add(new AuthenticateDTO { Id = 2, Login = "gsw", Password = "gsw#123", Role = "admin" });
            users.Add(new AuthenticateDTO { Id = 3, Login = "admin", Password = "admin#123", Role = "admin" });
            return users.Where(x => x.Login.ToLower() == login.ToLower() && x.Password == password).FirstOrDefault();
        }
    }
}
