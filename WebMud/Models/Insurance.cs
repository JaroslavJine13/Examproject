using System.ComponentModel.DataAnnotations;

namespace WebMud.Models
{
    public class Insurance
    {

        public Guid Id { get; set; }

        public Type TypeEnum { get; set; }

        [StringLength(1000)]
        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

 
        [Range(0, 9999999999999999.99)]
        [Required(ErrorMessage = "price is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Date From is required.")]
        public DateTime? DateFrom { get; set; }

        [Required(ErrorMessage = "Date To is required.")]
        public DateTime? DateTo { get; set; }

        public string ClientLink { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public string CreatorID { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
