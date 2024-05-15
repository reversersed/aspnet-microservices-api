using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions.RabbitExtension
{
    public static class RabbitNames
    {
        public const string RabbitHostname = "amqp://rmuser:rmpassword@rabbitmq:5672/";
        public const string OnUsernameUpdatedExchange = "user_updated_exchange";
    }
}
