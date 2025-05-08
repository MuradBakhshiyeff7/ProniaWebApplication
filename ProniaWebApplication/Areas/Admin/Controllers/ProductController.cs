using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProniaWebApplication.DAL;
using ProniaWebApplication.Models;

namespace ProniaWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                return View(product);
            }

            if (product.MainImageFile == null || product.HoverImageFile == null)
            {
                ModelState.AddModelError("", "Doldurulmalidi");
                return View(product);
            }

            string mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(product.MainImageFile.FileName);
            string hoverImageName = Guid.NewGuid().ToString() + Path.GetExtension(product.HoverImageFile.FileName);

            string mainImagePath = Path.Combine(_env.WebRootPath, "assets", "images", "product", "large-size", mainImageName);
            string hoverImagePath = Path.Combine(_env.WebRootPath, "assets", "images", "product", "large-size", hoverImageName);

            using (FileStream stream = new FileStream(mainImagePath, FileMode.Create))
            {
                product.MainImageFile.CopyTo(stream);
            }

            using (FileStream stream = new FileStream(hoverImagePath, FileMode.Create))
            {
                product.HoverImageFile.CopyTo(stream);
            }

            product.MainImage = mainImageName;
            product.HoverImage = hoverImageName;

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", product.CategoryId);
                return View(product);
            }

            var existing = _context.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null) return NotFound();

            if (product.MainImageFile != null)
            {
                string mainImageName = Guid.NewGuid().ToString() + Path.GetExtension(product.MainImageFile.FileName);
                string mainImagePath = Path.Combine(_env.WebRootPath, "assets", "images", "product", "large-size", mainImageName);

                using (FileStream stream = new FileStream(mainImagePath, FileMode.Create))
                {
                    product.MainImageFile.CopyTo(stream);
                }

                existing.MainImage = mainImageName;
            }

            if (product.HoverImageFile != null)
            {
                string hoverImageName = Guid.NewGuid().ToString() + Path.GetExtension(product.HoverImageFile.FileName);
                string hoverImagePath = Path.Combine(_env.WebRootPath, "assets", "images", "product", "large-size", hoverImageName);

                using (FileStream stream = new FileStream(hoverImagePath, FileMode.Create))
                {
                    product.HoverImageFile.CopyTo(stream);
                }

                existing.HoverImage = hoverImageName;
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Description = product.Description;
            existing.CategoryId = product.CategoryId;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
