namespace MeterReaderAPI.Helpers
{
    public class SysDataTablePager<T>
    {
        public SysDataTablePager(IEnumerable<T> items, int totalRecords, int totalDisplayRecords, int page)
        {
            this.aaData = items;
            this.iTotalRecords = totalRecords;
            this.iTotalDisplayRecords = totalDisplayRecords;
            this.page = page;
        }

        public IEnumerable<T> aaData { get; set; }

        public int iTotalRecords { get; set; }

        public int iTotalDisplayRecords { get; set; }

        public int page { get; set; }
    }
}
