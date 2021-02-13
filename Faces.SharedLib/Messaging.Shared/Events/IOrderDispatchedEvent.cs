using System;

namespace MessagingShared.Events
{
    public interface IOrderDispatchedEvent
    {
        DateTime DispatchDateTime { get; set; }
        Guid OrderId { get; set; }
    }
}