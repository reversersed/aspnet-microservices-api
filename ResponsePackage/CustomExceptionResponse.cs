using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsePackage
{
    public class CustomExceptionResponse(ResponseCodes _code, string _message, List<string>? _errors = null) : Exception(_message)
    {
        public readonly ResponseCodes Code = _code;
        public string CodeMessage { get { return Code.ToString(); } }
        public List<string>? Errors { get; set; } = _errors;
    }
}
