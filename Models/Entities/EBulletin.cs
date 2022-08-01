using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication_MyNoteSampleApp.Models.Entities
{
    [Table("EBulletins")]
    public class EBulletin 
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50), Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }

        [Display(Name = "Yasaklı")]
        public bool Banned { get; set; }

        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedAt { get; set; }
    }
    }
