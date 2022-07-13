using Microsoft.AspNetCore.Mvc;
using WebApplication_MyNoteSampleApp.Business;
using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController()
        {
            _categoryService = new CategoryService();
        }

        public IActionResult Index()
        {
             
            return View(_categoryService.List().Data);
        }


        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<Category> result = _categoryService.Create(model, HttpContext);
                if (!result.IsError)
                    return RedirectToAction(nameof(Index));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }


            return View(model);

        }
        public IActionResult Edit(int id)
        {
            ServiceResult<Category> result = _categoryService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            CategoryViewModel model = new CategoryViewModel
            {
                Name = result.Data.Name,
                Description = result.Data.Description
              
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(int id,CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<Category> result = _categoryService.Update(id, model, HttpContext);
                if (!result.IsError)
                    return RedirectToAction(nameof(Index));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }


            return View(model);
        }
        public IActionResult Delete()
        {
            return View();

        }
        public IActionResult Details()
        {
            return View();

        }
    }
}
