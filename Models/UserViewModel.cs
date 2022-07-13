using System.ComponentModel.DataAnnotations;

namespace WebApplication_MyNoteSampleApp.Models
{
    public class UserViewModel
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

        [Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(16, MinimumLength = 6, ErrorMessage = "{0} alanı maksimum {1} minimum {2} karakter olabilir"),
            Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(16, MinimumLength = 6, ErrorMessage = "{0} alanı maksimum {1} minimum {2} karakter olabilir"),
            Display(Name = "Şifre(Tekrar)")
            , Compare(nameof(Password), ErrorMessage = "{0} alanı ile {1} alanı eşleşmiyor.")]
        public string RePassword { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        [Display(Name = "Yönetici")]
        public bool IsAdmin { get; set; }
    }

}
