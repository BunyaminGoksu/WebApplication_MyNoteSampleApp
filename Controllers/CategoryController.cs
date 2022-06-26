using Microsoft.AspNetCore.Mvc;

namespace WebApplication_MyNoteSampleApp.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Create()
        {
            return View();

        }
        public IActionResult Edit()
        {
            return View();

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
