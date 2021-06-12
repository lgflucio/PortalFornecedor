using System.Collections.Generic;

namespace ExceptionHandler.DTOs
{
    public class ExceptionDetailsDTO
    {
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public string Code { get; set; }
    }
}
