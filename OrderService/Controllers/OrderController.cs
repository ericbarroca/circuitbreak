using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrderService.Models;
using OrderService.Services;
using OrderService.Settings;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly IOrdersService _service;

        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger,
        IOrdersService service)
        {
            _logger = logger;
            _service = service;
            
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Order>> Get([FromRoute] int id)
        {
            return Ok(await _service.LoadOrder(id));
        }
    }
}
