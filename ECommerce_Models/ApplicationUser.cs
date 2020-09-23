using ECommerce_Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce_Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public List<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
    }
}
