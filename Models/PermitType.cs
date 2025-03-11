using System.ComponentModel.DataAnnotations;
namespace Permit_App.Models
{
    public class PermitType
    {
        [Key]
        public int PermitTypeId { get; set; }
        [Required]
        public string PermitTypeName { get; set; }
    }
}
