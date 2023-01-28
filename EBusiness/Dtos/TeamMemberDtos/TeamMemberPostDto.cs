using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace EBusiness.Dtos.TeamMemberDtos
{
    public class TeamMemberPostDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Position { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
