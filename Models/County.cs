using System.ComponentModel.DataAnnotations;

namespace Permit_App.Models
{
    public class County
    {
        [Key]
        public int CountyId { get; set; }
        [Required]
        public string CountyName { get; set; }
    }
}
