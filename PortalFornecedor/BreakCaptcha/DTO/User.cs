using System;
using System.Collections.Generic;
using System.Text;

namespace BreakCaptcha.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
