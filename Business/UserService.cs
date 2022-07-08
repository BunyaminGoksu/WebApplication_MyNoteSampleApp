using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Context;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Business
{
    public class UserService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult<object> Register(RegisterViewModel model)
        {

            ServiceResult<object> result = new ServiceResult<object>();

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

      
        
        
        public ServiceResult<User> Login(LoginViewModel model)
        {
            ServiceResult<User> result = new ServiceResult<User>();

            model.Username = model.Username.Trim().ToLower();

          //  var user = _db.Users.Any(x => x.Username.ToLower() == model.Username
          //&& x.Password == model.Password);


            var user = _db.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username
            && x.Password == model.Password);


            if (user == null)
            {
                result.AddError("Hatalı kullanıcı adı yada şifre");
            }
            else
            {
                user.Password =String.Empty;
                result.Data = user;
            }


            return result;
        }

        public ServiceResult<List<User>> List()
        {
            var users = _db.Users.ToList();
            users.ForEach(user => user.Password = String.Empty);

            ServiceResult<List<User>> result = new ServiceResult<List<User>>();

            result.Data = users;

            return result;

        }
    }
}
