using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_48
{
    public class CustomerOrderResult
    {
        public int customer_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int product_id { get; set; }
        public int product_quantity { get; set; }
        public decimal product_price { get; set; }
    }
}
