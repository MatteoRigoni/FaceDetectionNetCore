using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingShared.Events
{
    public class OrderDispatchedEvent : IOrderDispatchedEvent
    {
        public Guid OrderId { get; set; }
        public DateTime DispatchDateTime { get; set; }
    }
}
