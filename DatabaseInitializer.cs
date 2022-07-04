using WebApplication_MyNoteSampleApp.Models.Context;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp
{
    public class DatabaseInitializer
    {
        private DatabaseContext _context;
        private AppSettings _settings;
        public DatabaseInitializer(DatabaseContext context, AppSettings settings)
        {
            _context = context;
            _settings = settings;
        }
        public void Seed()
        {
            if (_context.Users.Any(x => x.Username == _settings.AdminUsername) == false)
            {
                _context.Users.Add(new User
                {
                    Username = _settings.AdminUsername,
                    Password = _settings.AdminPassword,
                    FullName = _settings.AdminPassword,
                    Email = _settings.AdminEmail,
                    IsActive = true,
                    IsAdmin = true,
                    CreatedUser = "default",
                    CreatedAt = DateTime.Now, 
                    ModifiedUser = "default"
                });
                _context.SaveChanges();
            }
        }
    }
}
