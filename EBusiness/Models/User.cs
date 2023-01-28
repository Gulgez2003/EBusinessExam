using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EBusiness.Models
{
    public class User:IdentityUser
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string LastName { get; set; }
    }
}
