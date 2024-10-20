namespace MeterReaderAPI.DTO.Track
{
    public class TrackGridItem
    {
        public int Id { get; set; }

        public int Called { get; set; }

        public int UnCalled { get; set; }

        public int NotebookNumber { get; set; }

        public string Desc { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
