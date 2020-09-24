using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ECommerce_Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
   
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
    
        public string PhotoPath { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNewArrival { get; set; }
        public List<CartProduct> CartProducts { get; set; }
        [NotMapped]
        public bool IsInCart { get; set; }
    }
}
