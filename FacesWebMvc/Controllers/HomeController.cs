using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FacesWebMvc.Models;
using MassTransit;
using System.IO;
using MessagingShared.Constants;
using MessagingShared.Commands;

namespace FacesWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusControl _busControl;

        public HomeController(ILogger<HomeController> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderViewModel model)
        {
            MemoryStream eory = new MemoryStream();
            using (var uploadedFile = model.File.OpenReadStream())
            {
                await uploadedFile.CopyToAsync(eory);
            }

            model.ImageData = eory.ToArray();
            model.ImageUrl = model.File.FileName;
            model.OrderId = Guid.NewGuid();
            var sendToUri = new Uri($"{RabbitMqMassiveTransitConstants.RabbitMqUri }/" +
                $"{RabbitMqMassiveTransitConstants.RegisterOrderCommandQueue}");

            var endPoint = await _busControl.GetSendEndpoint(sendToUri);
            await endPoint.Send<IRegisterOrderCommand>(
                new
                {
                    model.OrderId,
                    model.Email,
                    model.ImageData,
                    model.ImageUrl
                });
            ViewData["OrderId"] = model.OrderId;
            return View("Thanks");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
