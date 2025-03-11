using System.ComponentModel.DataAnnotations;

namespace Permit_App.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Country { get; set; }
        //public User User { get; set; }
    }
}
