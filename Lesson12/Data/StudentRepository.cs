using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using College_DB.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace College_DB.Data
{
    class StudentRepository
    {
        private readonly string _conn_str;
        public StudentRepository(string connection_str)
        {
            _conn_str = connection_str;
        }


        //public List<Student> GetAllStudent()
        //{

        //    var students = new List<Student>();
        //    SqlConnection connection = new SqlConnection(_conn_str);
        //    connection.Open();
        //    string sql = "SELECT Id, FirstName, Lastname, Age FROM Students";
        //    var command = new SqlCommand(sql, connection);
        //    var reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        students.Add(new Student
        //        {
        //            StudentId = reader.GetInt32(0),
        //            FirstName = reader.GetString(1),
        //            LastName = reader.GetString(2),
        //            Age = reader.GetInt32(3)
        //        });
        //    }
        //    connection.Close();
        //    return students;
        //}
        public List<Student> GetAllStudent2()
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection.Query<Student>("SELECT Id, FirstName, Lastname, Age FROM Students").ToList();
            }
            
        }
        //public List<Student> GetStudentById(string id)
        //{

        //    var students = new List<Student>();
        //    SqlConnection connection = new SqlConnection(_conn_str);
        //    connection.Open();
        //    string sql = "SELECT Id, FirstName, Lastname, Age FROM Students WHERE Id = @id";
        //    var command = new SqlCommand(sql, connection);
        //    command.Parameters.AddWithValue("@id", id);
        //    var reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        students.Add(new Student
        //        {
        //            StudentId = reader.GetInt32(0),
        //            FirstName = reader.GetString(1),
        //            LastName = reader.GetString(2),
        //            Age = reader.GetInt32(3)
        //        });
        //    }
        //    connection.Close();
        //    return students;
        //}
        public Student GetStudentById(string id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection.QueryFirstOrDefault<Student>("SELECT Id, FirstName, Lastname, Age FROM Students WHERE Id = @id",
                    new { @Id = id } );
            }
        }

        public void CreateStudent(string first_name, string last_name, string age, string group_id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = "INSERT INTO Students (FirstName, LastName, Age, GroupId) " +
                             "VALUES (@FirstName, @LastName, @Age, @GroupId)";

                connection.Execute(sql, new
                {
                    FirstName = first_name,
                    LastName = last_name,
                    Age = age,
                    GroupId = group_id
                });
            }
        }
    }
}
