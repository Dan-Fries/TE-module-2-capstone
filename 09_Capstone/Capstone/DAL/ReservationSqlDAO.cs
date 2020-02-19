using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationSqlDAO
    {

        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM reservation";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        reservations.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return reservations;
        }

        private static Reservation RowToObject(SqlDataReader rdr)
        {
            Reservation reservation = new Reservation();

            reservation.ReservationId = Convert.ToInt32(rdr["reservation_id"]);


            reservation.SiteId = Convert.ToInt32(rdr["site_id"]);


            reservation.Name = Convert.ToString(rdr["name"]);

            reservation.StartDate = Convert.ToDateTime(rdr["from_date"]);

            reservation.EndDate = Convert.ToDateTime(rdr["to_date"]);


            reservation.BookingDate = Convert.ToDateTime(rdr["create_date"]);


            return reservation;
        }
    }
}
