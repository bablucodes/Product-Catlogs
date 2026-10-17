
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        private static readonly List<Product> products = new();
        private static int nextId = 1;

        public IActionResult Index(string sortOrder = "")
        {
            ViewBag.SearchString = "";
            ViewBag.SortOrder = sortOrder;

            return View(SortProducts(products, sortOrder).ToList());
        }

        public IActionResult Search(string searchString, string sortOrder = "")
        {
            IEnumerable<Product> result = products;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                result = result.Where(p =>
                    p.Name.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase) ||
                    (p.Category != null &&
                     p.Category.Contains(searchString,
                        StringComparison.OrdinalIgnoreCase)));
            }

            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;

            return View("Index", SortProducts(result, sortOrder).ToList());
        }

        private IEnumerable<Product> SortProducts(
            IEnumerable<Product> source, string sortOrder)
        {
            return sortOrder switch
            {
                "price_asc" => source.OrderBy(p => p.Price),
                "price_desc" => source.OrderByDescending(p => p.Price),
                _ => source
            };
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            product.Id = nextId++;
            products.Add(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product updatedProduct)
        {
            if (!ModelState.IsValid)
                return View(updatedProduct);

            var product = products.FirstOrDefault(
                p => p.Id == updatedProduct.Id);

            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Category = updatedProduct.Category;
            product.StockQuantity = updatedProduct.StockQuantity;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            products.Remove(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
