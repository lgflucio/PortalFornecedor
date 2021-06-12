using System;
using System.Collections.Generic;
using System.Text;

namespace ReceiverApi.Responses
{
    public class ResponseToken
    {
        public string Access_Token { get; set; }
        public string Token_Type { get; set; }
        public int Expires_In { get; set; }
    }
}
