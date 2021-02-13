using MassTransit;
using MessagingShared.Commands;
using MessagingShared.Constants;
using MessagingShared.Events;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OrdersApi.Models;
using OrdersApi.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrdersApi.Consumers
{
    public class RegisterOrderCommandConsumer : IConsumer<IRegisterOrderCommand>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHubContext<OrderHub> _hubConntext;

        public RegisterOrderCommandConsumer(IOrderRepository orderRepo, IHttpClientFactory clientFactory, IHubContext<OrderHub> hubConntext)
        {
            _orderRepo = orderRepo;
            _clientFactory = clientFactory;
            _hubConntext = hubConntext;
        }
        public async Task Consume(ConsumeContext<IRegisterOrderCommand> context)
        {
            var result = context.Message;
            if (result.OrderId != null)
            {
                SaveOrder(result);
                await _hubConntext.Clients.All.SendAsync("UpdateOrders", "New order created", result.OrderId);

                var client = _clientFactory.CreateClient();
                Tuple<List<byte[]>, Guid> orderDetailData = await GetFacesFromFaceApiAsync(client, result.ImageData, result.OrderId);
                List<byte[]> faces = orderDetailData.Item1;
                Guid orderId = orderDetailData.Item2;

                SaveOrderDetails(orderId, faces);
                await _hubConntext.Clients.All.SendAsync("UpdateOrders", "Order processedd", result.OrderId);

                var sendToUri = new Uri($"{RabbitMqMassiveTransitConstants.RabbitMqUri }/" +
                $"{RabbitMqMassiveTransitConstants.NotificationServiceQueue}");

                var endPoint = await context.GetSendEndpoint(sendToUri);
                await endPoint.Send<IOrderProcessedEvent>(
                    new
                    {
                        context.Message.OrderId,
                        faces,
                        context.Message.Email
                    });
            }
        }

        private async Task<Tuple<List<byte[]>, Guid>> GetFacesFromFaceApiAsync(HttpClient client, byte[] imageData, Guid orderId)
        {
            var byteContent = new ByteArrayContent(imageData);
            Tuple<List<byte[]>, Guid> orderDetailData = null;

            byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            using (var response = await client.PostAsync("http://localhost:6000/api/faces?OrderId=" + orderId, byteContent))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                orderDetailData = JsonConvert.DeserializeObject<Tuple<List<byte[]>, Guid>>(apiResponse);
            }

            return orderDetailData;
        }

        private void SaveOrderDetails(Guid orderId, List<byte[]> faces)
        {
            var order = _orderRepo.GetOrderAsync(orderId).Result;
            if (order != null)
            {
                order.Status = Status.Processed;
                foreach (var face in faces)
                {
                    var orderDetail = new OrderDetail()
                    {
                        OrderId = orderId,
                        FaceData = face
                    };

                    order.OrderDetails.Add(orderDetail);
                }

                _orderRepo.UpdateOrder(order);
            }
        }

        private void SaveOrder(IRegisterOrderCommand result)
        {
            Order order = new Order()
            {
                OrderId = result.OrderId,
                Email = result.Email,
                ImageData = result.ImageData,
                PictureUrl = result.PictureUrl,
                Status = Status.Registered
            };

            _orderRepo.RegisterOrder(order).GetAwaiter().GetResult();
        }
    }
}
