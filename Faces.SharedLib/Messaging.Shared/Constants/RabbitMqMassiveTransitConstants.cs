using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingShared.Constants
{
    public class RabbitMqMassiveTransitConstants
    {
        public const string RabbitMqUri = "rabbitmq://rabbitmq";
        public const string UserName = "guest";
        public const string Password = "guest";
        public const string RegisterOrderCommandQueue = "register.order.command";
    }
}
