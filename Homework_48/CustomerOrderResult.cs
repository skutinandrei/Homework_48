using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_48
{
    public class CustomerOrderResult
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
