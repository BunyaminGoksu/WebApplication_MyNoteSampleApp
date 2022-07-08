using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication_MyNoteSampleApp.Business;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService = new UserService();

        public IActionResult Index()
        {
            ServiceResult<List<User>> result = _userService.List();
            return View(result.Data);
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
