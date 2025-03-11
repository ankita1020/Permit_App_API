namespace Permit_App.Models
{
    public class FormData
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get;set; }
        public string Country { get; set; }
        public int CountyId { get; set; }
        public int PermitTypeId { get; set; }
    }
}
