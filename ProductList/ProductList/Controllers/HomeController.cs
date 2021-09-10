﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductList.Domain;
using ProductList.Domain.Entities;
using ProductList.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ProductList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AppEFContext _context { get; set; }
        public HomeController(ILogger<HomeController> logger, AppEFContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = _context.Products
                .Include(i => i.ProductImages)
                .Select(x => new ProductViewModel
                {
                    Id=x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Images = x.ProductImages.Select(t => new ProductImageItemVM
                    {
                        Path = "/images/" + t.Name
                    }).ToList()
                });

            return View(model);
        }

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            Product product = new();
            product.Name = model.Name;
            product.Price = model.Price;

            _context.Products.Add(product);
            _context.SaveChanges();

            List<ProductImage> images = new List<ProductImage>();

            foreach (var item in model.Images)
            {
                string ext = Path.GetExtension(item.FileName);
                string fileName = Path.GetRandomFileName() + ext;

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "products", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    item.CopyTo(stream);
                }

                ProductImage pImage = new ProductImage
                {
                    Name = fileName,
                    Product = product
                };
                images.Add(pImage);
            }
            _context.ProductImages.AddRange(images);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult Deletee(int id)
        {
            var prodTodelete = _context.Products.FirstOrDefault(x => x.Id == id);

            var result = _context.ProductImages.Where(c => c.ProductId == id).ToList();

            ProductImageToDelete delete = new();
            delete.Id = prodTodelete.Id;
            delete.Name = prodTodelete.Name;
            delete.Price = prodTodelete.Price;
            delete.productViewModels = result;            

            if (prodTodelete != null)
            {
                return View(delete);
            }
            return NotFound();
        }

        [HttpPost]       
        public IActionResult Delete(int id)
        {
            var imageDel = _context.ProductImages.FirstOrDefault(x => x.ProductId == id);
            var prodDel = _context.Products.FirstOrDefault(w => w.Id == id);
            if (imageDel != null && prodDel != null)
            {
            

                _context.ProductImages.Remove(imageDel);
                _context.Products.Remove(prodDel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        #region Edit
        [HttpGet]
        
        public IActionResult Edit(int id)
        {
            
            var resitem = _context.Products.FirstOrDefault(x => x.Id == id);
            var resimageitem = _context.ProductImages.Where(c => c.ProductId == id).ToList();

            ProductImageToEdit modeledit = new();
            modeledit.Id = resitem.Id;
            modeledit.Name = resitem.Name;
            modeledit.Price = resitem.Price;
            modeledit.Image = resimageitem;

            if (resitem != null)
            {
                return View(modeledit);
            }
            return NotFound();            
        }

        [HttpPost]       
        public async Task<IActionResult> Edit(int id, ProductImageToEdit modeledit)
        {

            if (ModelState.IsValid)
            {
                var itemProd = _context.Products.FirstOrDefault(x => x.Id == id);

                itemProd.Name = modeledit.Name;
                itemProd.Price = modeledit.Price;                

                string fileName = string.Empty;
                foreach (var item in modeledit.Image)
                {
                    string ext = Path.GetExtension(item.Name);
                    fileName = Path.GetRandomFileName() + ext;

                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "products", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                    }                  
                }


                


               
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        #endregion






        #region
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
