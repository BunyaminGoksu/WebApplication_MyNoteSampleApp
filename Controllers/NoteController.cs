using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication_MyNoteSampleApp.Business;
using WebApplication_MyNoteSampleApp.Core;
using WebApplication_MyNoteSampleApp.Core.Filters;
using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Controllers
{
    [LoginFilter]

    public class NoteController : Controller
    {

        private readonly CategoryService _categoryService;
        private readonly NoteService _noteService;
        public NoteController()
        {
            _categoryService = new CategoryService();
            _noteService = new NoteService();
        }
        public IActionResult Index()
        {
           int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;


            return View(_noteService.List(userId,true).Data);
        }
        public IActionResult LikedList()
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            return View(_noteService.ListLikedNotes(userId).Data);
        }
        public IActionResult Create()
        {
            LoadCategorySelectListDataToViewData();

            return View();

        }

        private void LoadCategorySelectListDataToViewData()
        {
            List<Category> categories = _categoryService.List().Data;
            List<SelectListItem> selectListItems =
                categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

            ViewData["categories"] = selectListItems;
        }

        [HttpPost]
        public IActionResult Create(NoteViewModel model)
        {

            if (ModelState.IsValid)
            {
                ServiceResult<Note> result = _noteService.Create(model, HttpContext);
                if (!result.IsError)
                    return RedirectToAction(nameof(Index));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            List<Category> categories = _categoryService.List().Data;
            List<SelectListItem> selectListItems =
                categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

            ViewData["categories"] = selectListItems;

            return View(model);






        }
        public IActionResult Edit(int id)
        {
            ServiceResult<Note> result = _noteService.Find(id);

            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;


            if (result.Data == null || (result.Data !=null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }

            NoteViewModel model = new NoteViewModel
            {
                Title = result.Data.Title, 
                Summary = result.Data.Summary,
                Detail = result.Data.Detail,
                IsDraft= result.Data.IsDraft,
                CategoryId= result.Data.CategoryId

            };

            LoadCategorySelectListDataToViewData();
           
            return View(model);

        }

        [HttpPost]
        public IActionResult Edit(int id, NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<Note> result = _noteService.Update(id, model, HttpContext);
                if (!result.IsError)
                    return RedirectToAction(nameof(Index));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            LoadCategorySelectListDataToViewData();


            return View(model);
        }


        public IActionResult Delete(int id)
        {
            var result = _noteService.Find(id);

            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            if (result.Data == null || (result.Data != null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);

        }
       
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {

            ServiceResult<object> result = _noteService.Remove(id);
            if (!result.IsError)
                return RedirectToAction(nameof(Index));

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(_noteService.Find(id).Data);
        }
       
        
        public IActionResult Details(int id) 
        {
            ServiceResult<Note> result = _noteService.Find(id);

            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            if (result.Data == null || (result.Data != null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);

        }

        [HttpPost]
        public IActionResult AddCommentToNote(int id,string text)
        {

            ServiceResult<Note> result = _noteService.AddComment(id, text, HttpContext);


            return Json(new {hasError = result.IsError});

        }

        [HttpPost]
        public IActionResult removeComment(int id)
        {
            ServiceResult<Note> result = _noteService.removeComment(id);

            return Json(new { hasError = result.IsError });

        }

        [HttpPost]
        public IActionResult updateComment(int id,string text)
        {
            ServiceResult<Note> result = _noteService.updateComment(id,text, HttpContext);

            return Json(new { hasError = result.IsError });

        }

        [HttpPost]
        public IActionResult likeNote(int id)
        {
            ServiceResult<int> result = _noteService.changeNoteLikeState(id, HttpContext);

            return Json(new { hasError = result.IsError,likeCount = result.Data});

        }
    }
}
