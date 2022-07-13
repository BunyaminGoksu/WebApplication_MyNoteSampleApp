using System.ComponentModel.DataAnnotations;

namespace WebApplication_MyNoteSampleApp.Models
{
    public class UserEditViewModel
    {

        [StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir"),
         Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir"),
            Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir"),
            EmailAddress(ErrorMessage = "{0} alanına geçerli bir e-posta adresi giriniz"),
            Display(Name = "E-Posta")]
        public string Email { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        [Display(Name = "Yönetici")]
        public bool IsAdmin { get; set; }
    }

}
