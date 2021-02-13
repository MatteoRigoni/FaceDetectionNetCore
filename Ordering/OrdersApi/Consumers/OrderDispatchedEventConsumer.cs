using MassTransit;
using MessagingShared.Events;
using Microsoft.AspNetCore.SignalR;
using OrdersApi.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersApi.Consumers
{
    public class OrderDispatchedEventConsumer : IConsumer<IOrderDispatchedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IHubContext<OrderHub> _hubConntext;
        public OrderDispatchedEventConsumer(IOrderRepository orderRepository, IHubContext<OrderHub> hubConntext)
        {
            _orderRepository = orderRepository;
            _hubConntext = hubConntext;
        }
        
        public async Task Consume(ConsumeContext<IOrderDispatchedEvent> context)
        {
            var message = context.Message;
            Guid orderId = message.OrderId;
            UpdateDatabase(orderId);
            await _hubConntext.Clients.All.SendAsync("UpdateOrders", "Order dispatched", orderId);
        }

        private void UpdateDatabase(Guid orderId)
        {
            var order = _orderRepository.GetOrder(orderId);
            if (order != null)
            {
                order.Status = Models.Status.Sent;
                _orderRepository.UpdateOrder(order);
            }
        }
    }
}
