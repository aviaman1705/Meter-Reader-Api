using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.DTO.User
{
    public class UserDetailsDTO
    {
        [Required(ErrorMessage = "שם משתמש הוא שדה חובה")]
        [Display(Name = "שם משתמש")]
        [MinLength(2, ErrorMessage = "שם משתמש חייב להכיל 2 תווים לפחות")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "מייל הוא שדה חובה")]
        [Display(Name = "מייל")]
        [EmailAddress(ErrorMessage = "שדה מייל לא תקין")]
        public string Email { get; set; }


        [Required(ErrorMessage = "טלפון הוא שדה חובה")]
        [Display(Name = "טלפון")]
        [RegularExpression(@"[0]{1}[2-9]{1,2}\-?[0-9]{7}", ErrorMessage = "שדה טלפון חייב להיות עם קידומית מישראל")]
        public string Phone { get; set; }

        [Display(Name = "תמונה")]
        public string? Image { get; set; }

        public IFormFile? ImageFile { get; set; }


    }
}
