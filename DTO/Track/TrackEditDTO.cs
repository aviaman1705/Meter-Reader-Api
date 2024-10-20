using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.DTO.Track
{
    public class TrackEditDTO
    {
        public int Id { get; set; }

        [Display(Name = "נקרא")]
        [Required(ErrorMessage = "שדה {0} הוא שדה חובה")]
        [RegularExpression("([0-9]+)", ErrorMessage = "נא הזן ספרות תקינות")]
        public int Called { get; set; }

        [Display(Name = "לא נקרא")]
        [Required(ErrorMessage = "שדה {0} הוא שדה חובה")]
        [RegularExpression("([0-9]+)", ErrorMessage = "נא הזן ספרות תקינות")]
        public int UnCalled { get; set; }

        [Display(Name = "מ-תאריך")]
        [Required(ErrorMessage = "שדה {0} הוא שדה חובה")]
        public string FromDate { get; set; }

        [Display(Name = "עד-תאריך")]
        [Required(ErrorMessage = "שדה {0} הוא שדה חובה")]
        public string ToDate { get; set; }

        [Display(Name = "תיאור")]
        [Required(ErrorMessage = "שדה {0} הוא שדה חובה")]
        public string Desc { get; set; }

        [Display(Name = "מספר פנקס")]
        [Required(ErrorMessage = "חובה לבחור מספר פנקס")]
        public int NotebookId { get; set; }
    }
}
