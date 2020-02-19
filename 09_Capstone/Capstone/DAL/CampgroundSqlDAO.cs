using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Campground> GetAllCampgrounds()
        {
            // throw new NotImplementedException();

            List<Campground> campgrounds = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM campground";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        campgrounds.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return campgrounds;
        }


        ///// <summary>
        ///// Searches the system for an employee by first name or last name.
        ///// </summary>
        ///// <remarks>The search performed is a wildcard search.</remarks>
        ///// <param name="firstname"></param>
        ///// <param name="lastname"></param>
        ///// <returns>A list of employees that match the search.</returns>
        //public IList<Employee> Search(string firstname, string lastname)
        //{
        //    List<Employee> employees = new List<Employee>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            string sql = "SELECT * FROM employee WHERE last_name LIKE @lastname AND first_name LIKE @firstname";

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            cmd.Parameters.AddWithValue("@lastname", "%" + lastname + "%");
        //            cmd.Parameters.AddWithValue("@firstname", "%" + firstname + "%");

        //            SqlDataReader rdr = cmd.ExecuteReader();

        //            while (rdr.Read())
        //            {
        //                employees.Add(RowToObject(rdr));
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return employees;
        //}

        ///// <summary>
        ///// Gets a list of employees who are not assigned to any active projects.
        ///// </summary>
        ///// <returns></returns>
        //public IList<Employee> GetEmployeesWithoutProjects()
        //{
        //    List<Employee> employees = new List<Employee>();

        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            string sql = @"SELECT * from employee
        //                    left outer join project_employee
        //                        ON employee.employee_id = project_employee.employee_id
        //                       WHERE project_employee.project_id IS NULL";

        //            SqlCommand cmd = new SqlCommand(sql, conn);
        //            //cmd.Parameters.AddWithValue("@lastname", lastname);
        //            //cmd.Parameters.AddWithValue("@firstname", firstname);

        //            SqlDataReader rdr = cmd.ExecuteReader();

        //            while (rdr.Read())
        //            {
        //                employees.Add(RowToObject(rdr));
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //    return employees;

        //}
        private static Campground RowToObject(SqlDataReader rdr)
        {
            return new Campground
            {
                CampgroundId = Convert.ToInt32(rdr["campground_id"]),
                ParkId = Convert.ToInt32(rdr["park_id"]),
                OpenFromMonth = Convert.ToInt32(rdr["open_from_mm"]),
                OpenToMonth = Convert.ToInt32(rdr["open_to_mm"]),

                Name = Convert.ToString(rdr["name"]),

                DailyFee = Convert.ToDecimal(rdr["daily_fee"])
            };
        }
    }
}
