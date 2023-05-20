using System.ComponentModel.DataAnnotations;

namespace WebMud.Models
{
    public class Gallery
    {

        public Guid Id { get; set; }

        public string Path { get; set; }

        [StringLength(1000)]
        public string InternalNote { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string CreatorID { get; set; }

        [Required(ErrorMessage ="Date is required!")]
        public DateTime? CreatedDate { get; set; }

        public bool IsOnWelcome { get; set; }

        public string FolderLink { get; set; }

    }
}
