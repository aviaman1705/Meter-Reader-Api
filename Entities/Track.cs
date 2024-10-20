using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReaderAPI.Entities
{
    public class Track
    {
        public int Id { get; set; }

        public int Called { get; set; }

        public int UnCalled { get; set; }

        public string Desc { get; set; }

        public int NotebookId { get; set; }

        [ForeignKey("NotebookId")]
        public virtual Notebook Notebook { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
