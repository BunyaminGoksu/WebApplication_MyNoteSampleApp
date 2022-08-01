using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Context;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Business
{
    public class EBulletinService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult<object> Create(string email)
        {
            var result = new ServiceResult<object>();


            email = email.Trim().ToLower();



            if (_db.EBulletins.Any(x => x.Email.ToLower() == email))
            {
                result.AddError($"'{email}' adresi zaten kayıtlıdır.");
                return result;
            }

            EBulletin eBulletin = new EBulletin
            {
                Email = email,
                Banned = false,
                CreatedAt = DateTime.Now
            };

            _db.EBulletins.Add(eBulletin);

            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
           

            return result;
        }

        public ServiceResult<List<EBulletin>> List()
        {
            var eBulletins = _db.EBulletins.ToList();

            ServiceResult<List<EBulletin>> result = new ServiceResult<List<EBulletin>>();

            result.Data = eBulletins;

            return result;

        }

        public ServiceResult<EBulletin> Find(int id)
        {
            ServiceResult<EBulletin> result = new ServiceResult<EBulletin>()
            {
                Data = _db.EBulletins.Find(id)
            };

            if (result.Data == null)
                result.AddError("Kayıt bulunamadı");


            return result;
        }


        public ServiceResult<EBulletin> Update(int id, EBulletinEditViewModel model)
        {

            var result = new ServiceResult<EBulletin>();

            model.Email = model.Email.Trim().ToLower();



            if (_db.EBulletins.Any(x => x.Email.ToLower() == model.Email && x.Id != id))
            {
                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır.");
                return result;
            }

            EBulletin bulletin = _db.EBulletins.Find(id);

            bulletin.Email = model.Email;
            bulletin.Banned= model.Banned;



            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = bulletin;

            }

            return result;

        }
        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            EBulletin eBulletin = _db.EBulletins.Find(id);

            if (eBulletin != null)
            {
                _db.EBulletins.Remove(eBulletin);

                if (_db.SaveChanges() == 0)
                {
                    result.AddError("Silme işlemi yapılamadı.");
                }

            }
            return result;
        }

    }
}
