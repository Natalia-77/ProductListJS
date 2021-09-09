using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductList.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductImageItemVM> Images { get; set; }
    }
    
    public class ProductImageItemVM
    {
        public string Path { get; set; }
    }

    public class AddProductViewModel
    {
        [Display(Name="Назва")]
        public string Name { get; set; }
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [Display(Name = "Фото")]
        public List<IFormFile> Images { get; set; }
    }
}
