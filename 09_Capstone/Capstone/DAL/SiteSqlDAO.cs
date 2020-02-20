using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {

        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Site> GetAllSites()
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

        public IList<Site> GetSitesByCampgroundId(int campgroundId)
        {
            List<Site> sites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM site WHERE campground_id = @campgroundId";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

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

        public IList<Site> GetAvailableSites(int campgroundId, DateTime startDate, DateTime endDate)
        {
            List<Site> sites = new List<Site>();

            int startMonth = startDate.Month;
            int endMonth = endDate.Month;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"SELECT TOP 5 * FROM site s
JOIN campground c ON s.campground_id = c.campground_id
WHERE s.campground_id = @campgroundId 
AND s.site_id NOT IN (SELECT site_id FROM reservation WHERE (from_date BETWEEN @startDate AND @endDate OR to_date BETWEEN @startDate AND @endDate))
AND (@startMonth BETWEEN c.open_from_mm AND c.open_to_mm)
AND (@endMonth BETWEEN c.open_from_mm AND c.open_to_mm)";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@startMonth", startMonth);
                    cmd.Parameters.AddWithValue("@endMonth", endMonth);

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

        public IList<Site> GetAvailableSitesAdvanced(int campgroundId, DateTime startDate, DateTime endDate, int maxOccupancyRequired, bool isAccessible, int rvSizeRequired, bool isHookupRequired)
        {
            List<Site> sites = new List<Site>();

            int startMonth = startDate.Month;
            int endMonth = endDate.Month;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"SELECT TOP 5 * FROM site s
JOIN campground c ON s.campground_id = c.campground_id
WHERE s.campground_id = @campgroundId 
AND s.site_id NOT IN (SELECT site_id FROM reservation WHERE (from_date BETWEEN @startDate AND @endDate OR to_date BETWEEN @startDate AND @endDate))
AND (@startMonth BETWEEN c.open_from_mm AND c.open_to_mm)
AND (@endMonth BETWEEN c.open_from_mm AND c.open_to_mm)
AND @maxOccupancyRequired <= s.max_occupancy
AND @rvSizeRequired <= s.max_rv_length
";
                    if (isAccessible)
                    {
                        sql += " AND s.accessible = 1 ";
                    }
                    if (isHookupRequired)
                    {
                        sql += " AND s.utility = 1";
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);
                    cmd.Parameters.AddWithValue("@startMonth", startMonth);
                    cmd.Parameters.AddWithValue("@endMonth", endMonth);
                    cmd.Parameters.AddWithValue("@maxOccupancyRequired", maxOccupancyRequired);
                    cmd.Parameters.AddWithValue("@rvSizeRequired", rvSizeRequired);

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

            site.SiteNumber = Convert.ToInt32(rdr["site_number"]);

            site.MaxOccupancy = Convert.ToInt32(rdr["max_occupancy"]);
           

            site.Accessible = Convert.ToBoolean(rdr["accessible"]);


            site.MaxRVLength = Convert.ToInt32(rdr["max_rv_length"]);

            site.Utilities = Convert.ToBoolean(rdr["utilities"]);

            return site;
        }
    }
}
