using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce_Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
        public int ProdcutId { get; set; }
    }
}
