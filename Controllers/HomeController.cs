using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication_MyNoteSampleApp.Business;
using WebApplication_MyNoteSampleApp.Core;
using WebApplication_MyNoteSampleApp.Core.Filters;
using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly NoteService _noteService;
        private readonly EBulletinService _eBulletinService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _userService = new UserService();
            _noteService = new NoteService();
            _eBulletinService = new EBulletinService();
        }

        public IActionResult Index(int? categoryId,string mode)
        {

           int userId =  HttpContext.Session.GetInt32(Constants.UserId).GetValueOrDefault();

            if(userId > 0)
            {
               var likedNoteIds= _noteService.GetUserLikedNoteIds(userId).Data;
                ViewData["likedNoteIds"] = likedNoteIds;
            }

            if (categoryId == null && string.IsNullOrEmpty(mode))            
                return View(_noteService.List(null,false).Data);           
            else          
                return View(_noteService.List(categoryId, mode,false).Data);

            

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<User> result = _userService.Login(model);
               
                if (!result.IsError)
                {
                    HttpContext.Session.SetInt32(Constants.UserId,result.Data.Id);
                    HttpContext.Session.SetString(Constants.UserName, result.Data.Username);
                    HttpContext.Session.SetString(Constants.UserEmail, result.Data.Email);
                    HttpContext.Session.SetString(Constants.UserRole, result.Data.IsAdmin ? "admin" : "member");
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

              
            }

            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
               ServiceResult<object> result =  _userService.Register(model);
                if (!result.IsError)
                    return RedirectToAction(nameof(Login));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }


            return View(model);
        }

        [LoginFilter]
        public IActionResult ProfileShow()
        {
            ProfileInfoEditViewModel model = GetProfileEditViewModel();
            return View(model);
        }

        [LoginFilter]
        public IActionResult ProfileEdit()
        {
            ProfileInfoEditViewModel model =  GetProfileEditViewModel();
            return View(model);
        }

        private ProfileInfoEditViewModel GetProfileEditViewModel()
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            var user = _userService.Find(userId).Data;

            ProfileInfoEditViewModel model = new ProfileInfoEditViewModel();
            model.Email = user.Email;
            model.FullName = user.FullName;
            model.Username = user.Username;

            return model;
        }

        [HttpPost]
        public IActionResult ProfileInfoSave(ProfileInfoEditViewModel info)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<object> result = (ServiceResult<object>)_userService.UpdateUserInfo(info, HttpContext);


                if (!result.IsError)
                {
                    ViewData["SuccessMessage"] = "Bilgileriniz kaydedilmiştir.";
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            else
            {
                return View(nameof(ProfileEdit), info);

            }


            ProfileInfoEditViewModel model = GetProfileEditViewModel();

            return View(nameof(ProfileEdit), model);

        }

        [HttpPost]
        public IActionResult ProfilePasswordChange(string newPassword)
        {
           var result = _userService.ChangePassword(newPassword, HttpContext);
          

            if (!result.IsError)
            {
                ViewData["SuccessMessage"] = "Şifre değişiminiz yapılmıştır";
            }
            else
            {
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            ProfileInfoEditViewModel model = GetProfileEditViewModel();


            return View(nameof(ProfileEdit),model);
        }

        [HttpPost]
        public IActionResult ProfileImageSave(IFormFile profileImage)
        {

            try
            {
                string uploadPath = Path.Combine(Environment.CurrentDirectory,"wwwroot", "uploads");
                
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;

                string fileName = $"profile_{userId}.jpg";
                string filePath = Path.Combine(uploadPath, fileName);

                using (FileStream stream = System.IO.File.Create(filePath)) 
                {
                    profileImage.CopyTo(stream);
                }
              
                ViewData["SuccessMessage"] = "Profil resminiz güncellenmiştir";

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Hata oluştu profil resmi güncellenemedi");
            }

            ProfileInfoEditViewModel model = GetProfileEditViewModel();

            return View(nameof(ProfileEdit),model);

           

        }

        [LoginFilter]
        public IActionResult DeleteProfile()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Unauthorize()
        {
            return View();
        }
        [LoginFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetNoteDetail(int id)
        {
            var result = _noteService.Find(id);


            if(result.Data == null)
                return NotFound(); //StatusCode : 404
            


            return PartialView("_NoteDetailPartial",result.Data);
        }


        public IActionResult GetNoteComment(int id)
        {
            var result = _noteService.Find(id);


            if (result.Data == null)
                return NotFound(); //StatusCode : 404


            result.Data.Comments.ForEach(c => c.ModifiedAt = c.ModifiedAt != null ? c.ModifiedAt : c.CreatedAt);
            result.Data.Comments = result.Data.Comments.OrderBy(c => c.ModifiedAt).ToList();

            return PartialView("_NoteCommentsPartial", result.Data);
        }

        [HttpPost]
        public IActionResult SaveEBulletinEmail(string eBulletinEmail)
        {
            ServiceResult<object> result = _eBulletinService.Create(eBulletinEmail);
            return RedirectToAction(nameof(Index));
        }


    }

}