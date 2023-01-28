using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace EBusiness.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Position { get; set; }
        [Required]
        public string ImageName { get; set; }
             
    }
}
