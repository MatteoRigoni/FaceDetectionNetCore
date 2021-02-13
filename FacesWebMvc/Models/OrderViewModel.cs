using FacesWebMvc.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacesWebMvc.Controllers
{
    public class OrderViewModel
    {
        [Display(Name="Order Id")]
        public Guid OrderId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Image file")]
        public IFormFile File { get; set; }
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }
        [Display(Name = "OrderStatus")]
        public string Status { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageString { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}