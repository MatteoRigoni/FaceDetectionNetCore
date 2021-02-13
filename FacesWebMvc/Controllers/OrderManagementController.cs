using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacesWebMvc.RestClients;
using Microsoft.AspNetCore.Mvc;

namespace FacesWebMvc.Controllers
{
    public class OrderManagementController : Controller
    {
        private readonly IOrderManagementApi _orderManagementApi;

        public OrderManagementController(IOrderManagementApi orderManagementApi)
        {
            _orderManagementApi = orderManagementApi;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderManagementApi.GetOrders();
            foreach (var order in orders)
            {
                order.ImageString = ConvertAndFormatToString(order.ImageData);
            }

            return View(orders);
        }

        [Route("/OrdDetails/{orderId}")]
        public async Task<IActionResult> OrdDetails(string orderId)
        {
            var order = await _orderManagementApi.GetOrderById(Guid.Parse(orderId));
            order.ImageString = ConvertAndFormatToString(order.ImageData);

            foreach (var detail in order.OrderDetails)
            {
                detail.ImageString = ConvertAndFormatToString(detail.FaceData);
            }

            return View(order);
        }


        private string ConvertAndFormatToString(byte[] imageData)
        {
            string imageData64 = Convert.ToBase64String(imageData);
            return string.Format("data:image/png;base64, {0}", imageData64);
        }
    }
}
