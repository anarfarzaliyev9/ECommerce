using AspNetCoreMVC_ECommerceApp.Areas.Admin.ViewModels;
using AutoMapper;
using ECommerce_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC_ECommerceApp.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductViewModel,Product >();
            CreateMap<Product, AddProductViewModel>();
            CreateMap<EditProductViewModel, Product>().ForMember(dest=>dest.Id,
                opt=>opt.MapFrom(src=>src.ProductId));
            CreateMap<Product, EditProductViewModel>();
        }
    }
}
