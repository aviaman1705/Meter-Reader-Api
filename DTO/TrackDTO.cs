using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReaderAPI.DTO
{
    public class TrackDTO
    {
        public int Id { get; set; }

        public int Called { get; set; }

        public int UnCalled { get; set; }

        public string Desc { get; set; }

        public int NotebookId { get; set; }

        public DateTime FromDate { get; set; }
    
        public DateTime ToDate { get; set; }        
    }
}
