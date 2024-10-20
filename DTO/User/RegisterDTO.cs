using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.DTO.User
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(2)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "City is required and must be properly formatted.")]
        public string Password { get; set; }
    }
}
