using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Park> GetAllParks()
        {
            // throw new NotImplementedException();

            List<Park> parks = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM park";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        parks.Add(RowToObject(rdr));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return parks;
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
        private static Park RowToObject(SqlDataReader rdr)
        {
            Park park = new Park
            {
                ParkId = Convert.ToInt32(rdr["park_id"]),
                Area = Convert.ToInt32(rdr["area"]),
                Visitors = Convert.ToInt32(rdr["visitors"]),

                Name = Convert.ToString(rdr["name"]),
                Location = Convert.ToString(rdr["location"]),
                Description = Convert.ToString(rdr["description"]),

                EstablishDate = Convert.ToDateTime(rdr["establish_date"])
            };

            return park;
        }
    }
}

