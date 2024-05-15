using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.RabbitExtension.EventModels
{
    public class UsernameChangeModel
    {
        public required string OldName { get; set; }
        public required string NewName { get; set; }
    }
}
