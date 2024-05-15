using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsePackage
{
    public class AuthenticationResponse
    {
        public required string Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
