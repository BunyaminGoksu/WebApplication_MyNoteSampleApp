using System.ComponentModel.DataAnnotations;

namespace WebApplication_MyNoteSampleApp.Models
{
	public class EBulletinEditViewModel
    {
        [Required(ErrorMessage = "{0} alanı boş geçilemez"),
         StringLength(50, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir"),
         EmailAddress(ErrorMessage = "{0} geçerli bir E-posta adresi olmalıdır."),
         Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }

        [Display(Name = "Yasaklı")]
        public bool Banned { get; set; }
    }
}
