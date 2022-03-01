using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutShopping.Models
{
    public class Order_all
    {
        public Order order { get; set; }
        public Order_info[] order_info { get; set; }

        public Order_subinfo[] order_subinfo { get; set; }
    }
}