using System;
using System.Collections.Generic;
using System.Text;

namespace BreakCaptcha.Responses
{
    public class ResponseSolved
    {
        public string Id { get; set; }
        public bool captchaSolved { get; set; }
        public string captchaText { get; set; }
    }
}
