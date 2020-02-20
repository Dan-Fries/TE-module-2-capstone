﻿using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
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

        public IList<Reservation> GetAllReservationsByCampgroundId(int campgroundId)
        {
            List<Reservation> reservations = new List<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql =
@"SELECT * FROM reservation r
JOIN site s ON r.site_id = s.site_id
WHERE s.campground_id = @campgroundId";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);

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

        public int MakeReservation(int siteNumber, int campgroundId, string name, DateTime startDate, DateTime endDate)
        {
            int newReservationId = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    string sql =
    $@"INSERT INTO reservation (site_id, name, from_date, to_date, create_date)
Values ((SELECT site_id FROM site WHERE site_number = @siteNumber AND campground_id = @campgroundId), @name, @fromDate, @toDate, @createDate)
Select @@identity;";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@siteNumber", siteNumber);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);
                    cmd.Parameters.AddWithValue("@createDate", DateTime.Now);

                    newReservationId = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }

            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return newReservationId;
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
