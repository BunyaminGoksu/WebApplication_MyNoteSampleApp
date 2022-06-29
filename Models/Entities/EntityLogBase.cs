using System.ComponentModel.DataAnnotations;

namespace WebApplication_MyNoteSampleApp.Models.Entities
{
    public class EntityLogBase
    {
        [Required, StringLength(30)]
        public string CreatedUser { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(50)]
        public string ModifiedUser { get; set; }

        public DateTime? ModifiedAt { get; set; }
    }
}
