using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_DB
{
    class Program
    {

        static void Main(string[] args)
        {

        }
        static string connection()
        {
            string connectionString =
            "Data Source=COMP11A1\\SQLEXPRESS;" +
            "Initial Catalog=Primer;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

            return connectionString;
        }
        static void ShowAllStudent()
        {
            SqlConnection connection
        }
    }
}
