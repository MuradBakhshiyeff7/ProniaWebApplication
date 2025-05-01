using Microsoft.AspNetCore.Mvc;
using ProniaWebApplication.DAL;
using ProniaWebApplication.Models;
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
            var products = _context.Products.ToList(); 
            return View(products);
        }
    }
}
