using System.ComponentModel.DataAnnotations;

namespace WebMud.Models
{
    public class Client
    {

        public Guid Id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        [StringLength(255)]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Adress is required.")]
        public string Adress { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "PSC is required.")]
        public string PSC { get; set; }

        [StringLength(1000)]
        public string InternalNote { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string CreatorID { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
