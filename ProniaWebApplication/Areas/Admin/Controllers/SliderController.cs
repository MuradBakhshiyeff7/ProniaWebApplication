using Microsoft.AspNetCore.Mvc;
using ProniaWebApplication.DAL;
using ProniaWebApplication.Models;
using ProniaWebApplication.ViewModels;

namespace ProniaWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public SliderController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        public IActionResult Index()
        {
            var sliders = _context.Sliders.ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SliderVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.ImageFile == null || !vm.ImageFile.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("ImageFile", "Duzgun dakil edin");
                return View(vm);
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
            string path = Path.Combine(_env.WebRootPath, "assets/images/slider/slide-img", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                vm.ImageFile.CopyTo(stream);
            }

            Slider slider = new Slider
            {
                Title = vm.Title,
                Subtitle = vm.Subtitle,
                Image = fileName,
                RedirectUrl = vm.RedirectUrl,
                IsActive = vm.IsActive
            };

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();

            var vm = new SliderVM
            {
                Id = slider.Id,
                Title = slider.Title,
                Subtitle = slider.Subtitle,
                Image = slider.Image,
                RedirectUrl = slider.RedirectUrl,
                IsActive = slider.IsActive
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, SliderVM vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();

            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Duzgun daxil edin");
                    return View(vm);
                }

                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
                string newPath = Path.Combine(_env.WebRootPath, "assets/images/slider/slide-img", newFileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    vm.ImageFile.CopyTo(stream);
                }

                
                string oldPath = Path.Combine(_env.WebRootPath, "assets/images/slider/slide-img", slider.Image);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

                slider.Image = newFileName;
            }

            slider.Title = vm.Title;
            slider.Subtitle = vm.Subtitle;
            slider.RedirectUrl = vm.RedirectUrl;
            slider.IsActive = vm.IsActive;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "assets/images/slider/slide-img", slider.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
