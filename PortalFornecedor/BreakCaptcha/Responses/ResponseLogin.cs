using BreakCaptcha.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BreakCaptcha.Responses
{
    public class ResponseLogin
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
