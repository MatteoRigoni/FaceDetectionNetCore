using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingShared.Events
{
    public interface IOrderProcessedEvent
    {
        public Guid OrderId { get; set; }
        List<byte[]> Faces { get; set; }
        public string Email { get; set; }
    }
}
