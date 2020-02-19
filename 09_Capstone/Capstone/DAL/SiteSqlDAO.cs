using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteSqlDAO
    {

        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetSites()
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM site";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        sites.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sites;
        }

        private static Site RowToObject(SqlDataReader rdr)
        {
            Site site = new Site();

            site.SiteId = Convert.ToInt32(rdr["site_id"]);


            site.CampgroundId = Convert.ToInt32(rdr["campground_id"]);


            site.Name = Convert.ToString(rdr["name"]);

            site.SiteNumber = Convert.ToInt32(rdr["site_number"]);

            site.MaxOccupancy = Convert.ToInt32(rdr["max_occupancy"]);
           

            site.Accessible = Convert.ToBoolean(rdr["accessible"]);


            site.MaxRVLength = Convert.ToInt32(rdr["max_rv_length"]);

           site.Utilities = Convert.ToBoolean(rdr["utilities"]);

            return site;
        }
    }
}
