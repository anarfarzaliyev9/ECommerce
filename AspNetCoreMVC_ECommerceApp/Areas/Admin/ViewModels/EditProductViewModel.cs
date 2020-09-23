using ECommerce_Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC_ECommerceApp.Areas.Admin.ViewModels
{
    public class EditProductViewModel
    {
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }

        public Category Category { get; set; }
        public string CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string Details { get; set; }
        [NotMapped]
        public IFormFile FormFile { get; set; }
        public string PhotoPath { get; set; }
        [DisplayName("Do you want show this product as featured product ?")]
        public bool IsFeatured { get; set; }
        [DisplayName("Do you want show this product as new arrival product ?")]
        public bool IsNewArrival { get; set; }
        public List<Category> Categories { get; set; }
    }
}
