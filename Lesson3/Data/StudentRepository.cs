using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using College_DB.Models;
using Microsoft.Data.SqlClient;


namespace College_DB.Data
{
    class StudentRepository
    {
        private readonly string _conn_str;
        public StudentRepository(string connection_str)
        {
            _conn_str = connection_str;
        }
      

        public List<Student> GetAllStudent()
        {

            var students = new List<Student>();
            SqlConnection connection = new SqlConnection(_conn_str);
            connection.Open();
            string sql = "SELECT StudentId, FirstName, Lastname, Age FROM Students";
            var command = new SqlCommand(sql, connection);
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                students.Add( new Student
                {
                    StudentId = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Age = reader.GetInt32(3)
                });
            }
            connection.Close();
            return students;
        }
    }
}
