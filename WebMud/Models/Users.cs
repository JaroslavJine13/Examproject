using System.ComponentModel.DataAnnotations;

namespace WebMud.Models
{
    public class Users
    {

        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        //[Username(ErrorMessage = "Email already exists.")]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "First name is required.")]
        public string FName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Second name is required.")]
        public string SName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain one cap. letter, one lowercase letter, at least one digit and special char.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Password is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords does not match.")]
        public string Password2 { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; }

        //[Required(ErrorMessage = "User role is required.")]
        public bool IsAdmin { get; set; }

        public string? Token { get; set; }

        public bool IsVerified { get; set; }

        public bool IsDark { get; set; }

        public string AvatarPath { get; set; }

        public int TrafficCount { get; set; } = 0;

        public bool IsDeleted { get; set; }

        public bool IsAuthenticated { get; set; }

        //---

        public bool IsChatAvailable { get; set; }

        public bool IsCustomer { get; set; }
    }





}
