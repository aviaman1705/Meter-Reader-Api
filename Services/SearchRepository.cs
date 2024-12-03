using MeterReaderAPI.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Reflection.PortableExecutable;

namespace MeterReaderAPI.Services
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ApplicationDbContext _context;

        public SearchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SearchResult> Search(string term)
        {
            List<SearchResult> searchResults = new List<SearchResult>();

            string ConnectionString = _context.Database.GetConnectionString() ?? "";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand("SP_SearchResults", conn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@Term", term));

                // execute the command
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    // iterate through results, printing each to console
                    while (rdr.Read())
                    {
                        searchResults.Add(new SearchResult()
                        {
                            Type = (string)rdr["Type"],
                            Name = (string)rdr["Name"],
                            Link = (string)rdr["Link"]
                        });
                    }

                    rdr.Close();
                }
            }

            return searchResults;
        }
    }
}
