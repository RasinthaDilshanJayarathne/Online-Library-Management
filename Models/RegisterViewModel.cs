using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string UserType { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        [Display(Name = "Contact Number")]
        public string Contact_No { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([\w\.\-]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be between 5 and 50 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string ConfirmPassword { get; set; }
    }
}
