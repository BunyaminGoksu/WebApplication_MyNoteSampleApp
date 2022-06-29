using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication_MyNoteSampleApp.Models.Entities
{
    [Table("EmailMembership")]
    public class EmailMembership : EntityLogBase
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(60), EmailAddress, Display(Name = "E-Posta")]
        public string EmailAdress { get; set; }
    }

}
