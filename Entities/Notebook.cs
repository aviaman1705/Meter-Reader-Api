namespace MeterReaderAPI.Entities
{
    public class Notebook
    {
        public int Id { get; set; }
        public int Number { get; set; }           
        public virtual List<Track> Tracks { get; set; } 
    }
}
