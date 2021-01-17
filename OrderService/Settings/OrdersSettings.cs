using System.Collections.Generic;
using OrderService.Models;

namespace OrderService.Settings
{
    public class OrdersSettings
    {
        public IEnumerable<Order> Orders{get;set;}
    }
}