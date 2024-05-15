using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsePackage
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public IList<string> Scopes { get; set; }
        public IList<string> Roles { get; set; }
    }
}
