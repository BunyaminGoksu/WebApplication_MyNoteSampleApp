using Microsoft.EntityFrameworkCore;
using WebApplication_MyNoteSampleApp.Models.Entities;

namespace WebApplication_MyNoteSampleApp.Models.Context
{
    public class DatabaseContext:DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<LikedNote> LikedNotes{ get; set; }

        public DbSet<EmailMembership> EmailMemberships{ get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-PF3HH9L\\SQLSERVER2017EXP;Database=NotesDB;Trusted_Connection=true");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

    }
}
