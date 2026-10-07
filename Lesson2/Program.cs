using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace College_DB
{
    class Program
    {
        static string conn_str = Connect();
        static void Main(string[] args)
        {
            ShowAllStudent(conn_str);
            Console.WriteLine("Staret!");
            Console.ReadKey();

        }
        static string Connect()
        {
            string connectionString =
            "Data Source=COMP11A1\\SQLEXPRESS;" +
            "Initial Catalog=Primer;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

            return connectionString;
        }
        static void ShowAllStudent(string conn_str)
        {
            SqlConnection connection = new SqlConnection(conn_str);
            connection.Open();
            string sql = "SELECT * FROM dbo.Students";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            connection.Close();
        }
    }
}
