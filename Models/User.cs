using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Permit_App.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public int CountyId { get; set; }
        [Required]
        public int PermitTypeId { get; set; }
        [Required]
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public string AddressVerified { get; set; } = "N";
    }
}
