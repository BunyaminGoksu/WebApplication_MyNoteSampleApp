using System.ComponentModel.DataAnnotations;

namespace WebApplication_MyNoteSampleApp.Models
{
    public class ProfileInfoEditViewModel
    {
        [StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir"),
     Display(Name = "Ad Soyad")]
        public string FullName { get; set; }

  

        [Required(ErrorMessage = "{0} alanı boş geçilemez"),
            StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir"),
            EmailAddress(ErrorMessage = "{0} alanına geçerli bir e-posta adresi giriniz"),
            Display(Name = "E-Posta")]
        public string Email { get; set; }


        public string? Username { get; set; }
    }

}
