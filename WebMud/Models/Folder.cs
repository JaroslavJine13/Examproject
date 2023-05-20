
using System.ComponentModel.DataAnnotations;

namespace WebMud.Models
{
    public class Folder
    {

        public Guid Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Question is required.")]
        public string Name { get; set; }

        [StringLength(3000)]
        public string Note { get; set; }

        [StringLength(3000)]
        public string InternalNote { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string CreatorID { get; set; }

        public DateTime? CreatedDate { get; set; }




    }
}
