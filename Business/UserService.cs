using NETCore.Encrypt.Extensions;
using WebApplication_MyNoteSampleApp.Core;
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

            model.Email = model.Email.Trim().ToLower();

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
                Password = $"{Constants.EncrpytionSalt}{model.Password}".MD5(),
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

        public ServiceResult<User> Find(int id)
        {
            ServiceResult<User> result = new ServiceResult<User>()
            {
                Data = _db.Users.Find(id)
            };

            if (result.Data == null)
                result.AddError("Kayıt bulunamadı");
            else
                result.Data.Password = String.Empty;

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

        public ServiceResult<User> Update(int id, UserEditViewModel model, HttpContext httpContext)
        {

            var result = new ServiceResult<User>();

            model.Username = model.Username.Trim().ToLower();

            model.Email = model.Email.Trim().ToLower();



            if (_db.Users.Any(x => x.Email.ToLower() == model.Email.ToLower() && x.Id !=id))
            {
                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır.");
                return result;
            }
            if (_db.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
            {
                result.AddError($"'{model.Username}' kullanıcı adı zaten kayıtlıdır.");
                return result;

            }

            User user = _db.Users.Find(id);

            user.FullName = model.FullName;
            user.Username = model.Username;
            user.Email = model.Email;
            user.IsActive = model.IsActive;
            user.IsAdmin= model.IsAdmin;
            user.ModifiedUser = httpContext.Session.GetString(Constants.UserName);
            user.ModifiedAt = DateTime.Now;


            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = user;

            }

            return result;

        }

        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            User user = _db.Users.Find(id);

            if(user != null)
            {
                _db.Users.Remove(user);

                if (_db.SaveChanges() == 0)
                {
                    result.AddError("Silme işlemi yapılamadı.");
                }
               
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

        public ServiceResult<User> Create(UserViewModel model, HttpContext httpContext)
        {
            var result = new ServiceResult<User>();

            model.Username = model.Username.Trim().ToLower();

            model.Email= model.Email.Trim().ToLower();



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

            User user = new User
            {
                FullName = model.FullName,
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                IsActive = model.IsActive,
                IsAdmin = model.IsAdmin,
                CreatedAt = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.UserName)

            };

            _db.Users.Add(user);

            if(_db.SaveChanges()== 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = user;

            }

            return result;
        }

    }
}
