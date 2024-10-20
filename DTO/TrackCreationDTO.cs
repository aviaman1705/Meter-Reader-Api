using System.ComponentModel.DataAnnotations;

namespace MeterReaderAPI.DTO
{
    public class TrackCreationDTO
    {
        [Required(ErrorMessage ="the field with name {0} is required")]
        [RegularExpression("([0-9]+)",ErrorMessage = "Please enter valid Number")]
        public int Called { get; set; }

        [Required(ErrorMessage = "the field with name {0} is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid Number")]
        public int UnCalled { get; set; }

        [Required(ErrorMessage = "the field with name {0} is required")]
        [StringLength(250)]
        public string Desc { get; set; }

        [Required(ErrorMessage = "the field with name {0} is required")]
        [RegularExpression("([0-9999]+)", ErrorMessage = "Please enter valid Number")]
        public int NotebookId { get; set; }

        [Required(ErrorMessage = "the field with name {0} is required")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "the field with name {0} is required")]
        public DateTime ToDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
