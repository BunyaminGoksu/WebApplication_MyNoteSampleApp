using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Mail;
using WebApplication_MyNoteSampleApp.Business;
using WebApplication_MyNoteSampleApp.Core;
using WebApplication_MyNoteSampleApp.Core.Filters;
using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Controllers
{

    [LoginFilter]
    [AdminFilter]

    public class EBulletinController : Controller
    {

        private readonly EBulletinService _bulletinService;

        public EBulletinController()
        {
            _bulletinService = new EBulletinService();
        }

        public IActionResult Index()
        {
            return View(_bulletinService.List().Data);
        }


        public IActionResult Edit(int id)
        {
            ServiceResult<EBulletin> result = _bulletinService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            EBulletinEditViewModel model = new EBulletinEditViewModel
            {
                Email = result.Data.Email,
                Banned = result.Data.Banned

            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, EBulletinEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<EBulletin> result = _bulletinService.Update(id, model);
                if (!result.IsError)
                    return RedirectToAction(nameof(Index));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }


            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var result = _bulletinService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);

        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {

            ServiceResult<object> result = _bulletinService.Remove(id);
            if (!result.IsError)
                return RedirectToAction(nameof(Index));

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return View(_bulletinService.Find(id).Data);
        }

        public IActionResult SendEmails()
        {
            var bulletins = _bulletinService.ListExceptBanned().Data;

            var list = bulletins.Select(x => new SelectListItem(x.Email, x.Email)).ToList();

            EBulletinSendEmailsViewModel model = new EBulletinSendEmailsViewModel();
            model.EmailAdresses = new SelectList(list);

            return View(model);
        }
        [HttpPost]
        public IActionResult SendEmails(EBulletinSendEmailsViewModel model)
        {
            MailHelper mailHelper = new MailHelper();
            mailHelper.SendMail(model.Subject, model.Text, model.Emails.ToArray());

            return RedirectToAction(nameof(Index));
        }

    }
}
