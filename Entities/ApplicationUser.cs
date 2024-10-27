using Microsoft.AspNetCore.Identity;

namespace MeterReaderAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Image { get; set; }
    }
}
