using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Context;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Business
{
    public class UserService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult Register(RegisterViewModel model)
        {

            ServiceResult result = new ServiceResult();

            model.Username = model.Username.Trim().ToLower();


            if (_db.Users.Any(x => x.Email.ToLower() == model.Email.ToLower()))
            {
                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır.");   
                return result;
            }
            if (_db.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
            {
                result.AddError($"'{model.Username}' kullanıcı adı zaten kayıtlıdır.");
                return result;

            }
            _db.Users.Add(new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsActive = true,
                IsAdmin = false,
                CreatedUser = "register",
                CreatedAt = DateTime.Now,
            });

            if (_db.SaveChanges() == 0)
            {

                result.AddError("Kayıt yapılamadı");
            }

            return result;
        }

        public ServiceResult Login(LoginViewModel model)
        {
            ServiceResult result = new ServiceResult();

            model.Username = model.Username.Trim().ToLower();

            if (_db.Users.Any(x => x.Username.ToLower()== model.Username 
            && x.Password == model.Password)==false)
            {
                result.AddError("Hatalı kullanıcı adı yada şifre!");
            }


            return result;
        }
    }
}
