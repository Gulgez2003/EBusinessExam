using System.ComponentModel.DataAnnotations;

namespace EBusiness.Dtos.UserDtos
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string LastName { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
