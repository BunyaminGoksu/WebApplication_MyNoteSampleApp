using WebApplication_MyNoteSampleApp.Core;
using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Context;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Business
{
    public class CategoryService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult<List<Category>> List()
        {
            var categories = _db.Categories.ToList();

            ServiceResult<List<Category>> result = new ServiceResult<List<Category>>();

            result.Data = categories;

            return result;

        }


        public ServiceResult<Category> Create(CategoryViewModel model, HttpContext httpContext)
        {
            var result = new ServiceResult<Category>();

            model.Name= model.Name.Trim().ToLower();




            if (_db.Categories.Any(x => x.Name.ToLower() == model.Name.ToLower()))
            {
                result.AddError($"'{model.Name}' kategorisi zaten kayıtlıdır.");
                return result;
            }


            Category category= new Category
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.UserName)

            };

            _db.Categories.Add(category);

            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = category;

            }
            return result;
        }

        public ServiceResult<Category> Find(int id)
        {
            ServiceResult<Category> result = new ServiceResult<Category>()
            {
                Data = _db.Categories.Find(id)
            };

            if (result.Data == null)
                result.AddError("Kayıt bulunamadı");
         

            return result;
        }

        public ServiceResult<Category> Update(int id, CategoryViewModel model, HttpContext httpContext)
        {

            var result = new ServiceResult<Category>();

            model.Name = model.Name.Trim();




            if (_db.Categories.Any(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != id))
            {
                result.AddError($"'{model.Name}' isimli kategori zaten kayıtlıdır.");
                return result;
            }
           

            Category category = _db.Categories.Find(id);

           category.Name = model.Name;
           category.Description = model.Description;
          
           category.ModifiedUser = httpContext.Session.GetString(Constants.UserName);
           category.ModifiedAt = DateTime.Now;


            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = category;

            }

            return result;

        }

        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            Category category = _db.Categories.Find(id);

            if (category != null)
            {
                _db.Categories.Remove(category);

                if (_db.SaveChanges() == 0)
                {
                    result.AddError("Silme işlemi yapılamadı.");
                }

            }
            return result;
        }


    }
}
