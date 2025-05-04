using Microsoft.AspNetCore.Mvc;
using ProniaWebApplication.DAL;
using ProniaWebApplication.Models;
using ProniaWebApplication.ViewModels;
using System.Diagnostics;

namespace ProniaWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomeVM
            {
                Products = _context.Products.ToList(),
                Sliders = _context.Sliders.Where(s => s.IsActive).ToList()
            };

            return View(model);


        }
    }
}
