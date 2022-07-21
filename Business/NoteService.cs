using Microsoft.EntityFrameworkCore;
using WebApplication_MyNoteSampleApp.Core;
using WebApplication_MyNoteSampleApp.Models;
using WebApplication_MyNoteSampleApp.Models.Context;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Business
{
    public class NoteService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult<Note> Create(NoteViewModel model, HttpContext httpContext)
        {
            var result = new ServiceResult<Note>();

            Note note = new Note
            {
                Title = model.Title,
                Summary = model.Summary,
                Detail = model.Detail,
                IsDraft = model.IsDraft,
                CategoryId = model.CategoryId,
                OwnerId = httpContext.Session.GetInt32(Constants.UserId).Value,
                CreatedAt = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.UserName)

            };

            _db.Notes.Add(note);

            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = note;

            }
            return result;
        }

        public ServiceResult<List<Note>> List(int? userId)
        {
            IQueryable<Note> notes;

            notes = _db.Notes
          .Include(n => n.Likes)
          .Include(n => n.Comments)
          .Include(n => n.Category)
          .AsQueryable();


            if (userId != null)
            {
                notes = notes.Where(n => n.OwnerId == userId);
            }

            ServiceResult<List<Note>> result = new ServiceResult<List<Note>>();

            result.Data = notes.ToList();

            return result;

        }

        public ServiceResult<List<Note>> ListLikedNotes(int? userId)
        {
            IQueryable<Note> notes;

            var likes = _db.LikedNotes.Where(x => x.UserId == userId && x.NoteId != null).ToList();

            List<int> likedNotes = likes.Select(x => x.NoteId.Value).ToList();

            notes = _db.Notes
                 .Where(n => likedNotes.Contains(n.Id))
                 .Include(n => n.Likes)
                 .Include(n => n.Comments)
                 .Include(n => n.Category)
                 .AsQueryable();
        

            
            ServiceResult<List<Note>> result = new ServiceResult<List<Note>>();

            result.Data = notes.ToList();

            return result;

        }


        public ServiceResult<Note> Find(int id)
        {
            ServiceResult<Note> result = new ServiceResult<Note>()
            {
                Data = _db.Notes
                .Include(n => n.Likes)
                .Include(n => n.Comments)
                .Include(n => n.Category)
                .SingleOrDefault(n => n.Id == id)
            };

            if (result.Data == null)
                result.AddError("Kayıt bulunamadı");


            return result;
        }

        public ServiceResult<Note> Update(int id, NoteViewModel model, HttpContext httpContext)
        {

            var result = new ServiceResult<Note>();



            Note note = _db.Notes.Find(id);

            note.Title = model.Title;
            note.Summary = model.Summary;
            note.Detail = model.Detail;
            note.IsDraft = model.IsDraft;
            note.CategoryId = model.CategoryId;
            note.ModifiedUser = httpContext.Session.GetString(Constants.UserName);
            note.ModifiedAt = DateTime.Now;


            if (_db.SaveChanges() == 0)
            {
                result.AddError("Kayıt yapılamadı.");
            }
            else
            {
                result.Data = note;

            }

            return result;
        }

        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            Note note = _db.Notes.Find(id);

            if (note != null)
            {
                _db.Notes.Remove(note);

                if (_db.SaveChanges() == 0)
                {
                    result.AddError("Silme işlemi yapılamadı.");
                }

            }
            return result;
        }
    }
}
