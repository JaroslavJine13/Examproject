using System.ComponentModel.DataAnnotations;

namespace WebMud.Models
{
    public class Tickets
    {

        public Guid Id               { get; set;}

        [Required(ErrorMessage = "Email je vyžadován!")]
        [StringLength(255)]
        public string Email          { get; set;}


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(255)]
        public string Company        { get; set;}


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50)]
        public string Fname          { get; set;}


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50)]
        public string Sname          { get; set;}

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(50)]
        public string Phone          { get; set;}


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(10000)]
        public string Task           { get; set;}

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [StringLength(10000)]
        public string InternalNote   { get; set;}

       public bool IsNoticed      { get; set;}

       public bool IsCompleted    { get; set;}

       public bool IsDeleted      { get; set;}

       public string? AssignedUserID { get; set;}

       public DateTime? CreatedDate    { get; set;}

       public DateTime? CompletedDate  { get; set;}

        public DateTime? AssignedDate { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "GDPR!")]
        public bool IsGDPR { get; set; } = false;
    }
}
