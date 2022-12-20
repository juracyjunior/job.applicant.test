﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestHB.Domain.Commands;
using BestHB.Domain.Queries;
using BestHB.Domain.Repositories;
using BestHB.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using BestHB.Domain.Services;

namespace BestHB.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        public readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody]CreateOrderCommand command)
        {
            try
            {
                var orderId = _orderService.CreateAsync(command);
                return Ok(orderId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult Delete([FromBody]DeleteOrderCommand command)
        {
            try
            {
                var status = _orderService.DeleteAsync(command);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> Get([FromBody]QueryOrders queryOrders)
        {
            try
            {
                var orders = await _orderService.GetAsync(queryOrders);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("csv")]
        public async Task<IActionResult> GetAsCSV([FromBody] QueryOrders queryOrders)
        {
            try
            {
                return Ok(await _orderService.AsCsvAsync(queryOrders));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}