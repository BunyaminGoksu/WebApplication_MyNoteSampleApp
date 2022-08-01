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

        public ServiceResult<List<int>> GetUserLikedNoteIds(int userId)
        {
            ServiceResult<List<int>> result = new ServiceResult<List<int>>();

           result.Data =  _db.LikedNotes
                .Include(x => x.Note)
                .Where(x => x.UserId == userId && x.Note.IsDraft != true)
                .Select(x => x.NoteId.GetValueOrDefault())
                .ToList();

            return result;

        }

        public ServiceResult<List<Note>> List(int? categoryId, string mode, bool forAdminSide)
        {
            IQueryable<Note> notes;

            notes = _db.Notes
          .Include(n => n.Likes)
          .Include(n => n.Comments)
          .Include(n => n.Category)
          .Include(n => n.Owner)
          .AsQueryable();

            if (categoryId != null)
            {
                notes = notes.Where(n => n.CategoryId == categoryId);
            }


            if (forAdminSide == false)
            {
                notes = notes.Where(n => n.IsDraft == false);

            }



            List<Note> noteList = notes.AsNoTracking().ToList();
            noteList.ForEach(n => n.ModifiedAt = (n.ModifiedAt == null ? n.CreatedAt : n.ModifiedAt));

            if (string.IsNullOrEmpty(mode))
            {
                switch (mode)
                {
                    case "last":
                        noteList = noteList.OrderByDescending(n => n.ModifiedAt).ToList();
                        break;

                    case "mostliked":
                        noteList = noteList.OrderByDescending(n => n.Likes.Count).ToList();
                        break;

                    default:
                        break;
                }
            }

            ServiceResult<List<Note>> result = new ServiceResult<List<Note>>();

            result.Data = noteList;

            return result;
        }

        public ServiceResult<List<Note>> List(int? userId, bool forAdminSide)
        {
            IQueryable<Note> notes;

            notes = _db.Notes
          .Include(n => n.Likes)
          .Include(n => n.Comments)
          .Include(n => n.Category)
          .Include(n => n.Owner)
          .AsQueryable();


            if (userId != null)
            {
                notes = notes.Where(n => n.OwnerId == userId);
            }

            if (forAdminSide == false)
            {
                notes = notes.Where(n => n.IsDraft == false);

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
                 .Where(n => likedNotes.Contains(n.Id) && (n.OwnerId == userId || (n.OwnerId != userId && n.IsDraft == false)))
                 .Include(n => n.Likes)
                 .Include(n => n.Comments)
                 .Include(n => n.Category)
                 .Include(n => n.Owner)

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
                .Include(n => n.Owner)
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

        public ServiceResult<int> changeNoteLikeState(int id, HttpContext httpContext)
        {
            ServiceResult<int> result = new ServiceResult<int>();

            int userId = httpContext.Session.GetInt32(Constants.UserId).Value;

            LikedNote like = _db.LikedNotes.SingleOrDefault(x => x.NoteId == id && x.UserId == userId);

            if(like != null)
            {
                _db.LikedNotes.Remove(like);
            }
            else
            {
                _db.LikedNotes.Add(new LikedNote
                {
                    UserId = userId,
                    NoteId = id
                });
            }

            if (_db.SaveChanges() == 0)
            {
                result.AddError("İşlem başarısız.");
            }
            else
            {
                result.Data = _db.LikedNotes.Count(x => x.NoteId == id);
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

        public ServiceResult<Note> removeComment(int id)
        {
            var result = new ServiceResult<Note>();
            var comment = _db.Comments.Find(id);

            _db.Comments.Remove(comment);

            if (_db.SaveChanges() == 0)
            {
                result.AddError("Yorum silinemedi.");
            }
            else
            {
                result.Data = Find(id).Data;

            }

            return result;
        }

        public ServiceResult<Note> updateComment(int id, string text, HttpContext httpContext)
        {
            var result = new ServiceResult<Note>();
            var comment = _db.Comments.Find(id);

            comment.Text = text;
            comment.ModifiedUser = httpContext.Session.GetString(Constants.UserName);
            comment.ModifiedAt = DateTime.Now;

            if (_db.SaveChanges() == 0)
            {
                result.AddError("Yorum güncellenemedi.");
            }
            else
            {
                result.Data = Find(id).Data;

            }

            return result;
        }

        public ServiceResult<Note> AddComment(int id, string commentText, HttpContext httpContext)
        {
            var result = new ServiceResult<Note>();

            _db.Comments.Add(new Comment
            {
                NoteId = id,
                UserId = httpContext.Session.GetInt32(Constants.UserId),
                Text = commentText,
                CreatedAt = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.UserName)

            });

            if (_db.SaveChanges() == 0)
            {
                result.AddError("Yorum eklenemedi.");
            }
            else
            {
                result.Data = Find(id).Data;

            }

            return result;
        }
    }
}
