namespace Permit_App.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class UspsService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly string _uspsUserId;

        public UspsService(HttpClient httpClient, AppDbContext context, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _context = context;
            _uspsUserId = "6PERMI5601K49";//configuration["USPS:UserId"]; // Fetch USPS User ID from config
        }

        public async Task<bool> VerifyAndUpdateAddressAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null || user.Address == null)
            {
                return false; // User or Address not found
            }

            string uspsUrl = $"https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" +
                             $"<AddressValidateRequest USERID='{_uspsUserId}'>" +
                             $"<Address><Address1>{user.Address.AddressLine2}</Address1>" +
                             $"<Address2>{user.Address.AddressLine1}</Address2>" +
                             $"<City>{user.Address.City}</City><State>{user.Address.State}</State>" +
                             $"<Zip5>{user.Address.ZipCode}</Zip5><Zip4></Zip4></Address>" +
                             $"</AddressValidateRequest>";

            var response = await _httpClient.GetAsync(uspsUrl);
            if (!response.IsSuccessStatusCode) return false;

            var xmlResponse = await response.Content.ReadAsStringAsync();
            bool isValid = ParseUspsResponse(xmlResponse);

            // Update user record if the address is verified
            if (isValid)
            {
                user.AddressVerified = "Y";
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            return isValid;
        }

        private bool ParseUspsResponse(string xmlResponse)
        {
            try
            {
                var doc = XDocument.Parse(xmlResponse);
                var error = doc.Descendants("Error").FirstOrDefault();
                if (error != null) return false; // Address is invalid if there is an error

                var address2 = doc.Descendants("Address2").FirstOrDefault()?.Value;
                var city = doc.Descendants("City").FirstOrDefault()?.Value;
                var state = doc.Descendants("State").FirstOrDefault()?.Value;

                return !string.IsNullOrWhiteSpace(address2) && !string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(state);
            }
            catch (Exception)
            {
                return false; // Invalid XML response
            }
        }
    }
}
