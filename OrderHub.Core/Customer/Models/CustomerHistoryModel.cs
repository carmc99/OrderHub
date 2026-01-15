using OrderHub.Core.Order.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderHub.Core.Customer.Models
{
    public class CustomerHistoryModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerAddress { get; set; }
        public List<OrderHistoryItemModel> Orders { get; set; } = new();
        public int TotalOrders { get; set; }
        public double TotalCompletedAmount { get; set; }
    }
}
