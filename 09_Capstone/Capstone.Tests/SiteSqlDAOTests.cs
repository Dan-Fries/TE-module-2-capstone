using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class SiteSqlDAOTests
    {
        private TransactionScope transaction = null;

        private string connectionString = "Server=.\\SqlExpress;Database=npcampground;Trusted_Connection=True;";
        private int newDepartmentId;


        [TestInitialize]
        public void SetupDatabase()
        {
            //Start a transaction so we can rollback when we are finished with this test
            transaction = new TransactionScope();

            //Open Setup.sql and read in the script to be executed 
            string setupSQL;
            using (StreamReader rdr = new StreamReader("Setup.sql"))
            {
                setupSQL = rdr.ReadToEnd();
            }

            //Connect to the DB and execute the script
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(setupSQL, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    newDepartmentId = Convert.ToInt32(rdr["newDepartmentId"]);
                }
            }
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            //Rollback the transaction to get our good data back
            transaction.Dispose();
        }

        [TestMethod]
        public void TestGetDepartments()
        {
            //Arrange
            DepartmentSqlDAO dao = new DepartmentSqlDAO(connectionString);

            //Act
            IList<Department> departments = dao.GetDepartments();
            int i = 0;
            for (; i < departments.Count; i++)
            {
                if (departments[i].Id == newDepartmentId)
                {
                    break;
                }
            }

            //Assert 
            Assert.AreEqual(2, departments.Count);
            Assert.AreEqual("Cosmetics", departments[i].Name);
        }
    }
}
