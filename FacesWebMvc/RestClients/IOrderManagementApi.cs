﻿using FacesWebMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Refit;

namespace FacesWebMvc.RestClients
{
    public interface IOrderManagementApi
    {
        [Get("/orders")]
        Task<List<OrderViewModel>> GetOrders();

        [Get("/orders/{orderId}")]
        Task<OrderViewModel> GetOrderById(Guid orderId);
    }
}
