
using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace College_DB.Data
{
    public class StudentRepository
    {
        private readonly string _conn_str;

        public StudentRepository(string connection_string)
        {
            _conn_str = connection_string;
        }

        // GET ALL STUDENTS
        public List<Student> GetAllStudents()
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = @"
                    SELECT
                        Id AS StudentId,
                        FirstName,
                        LastName,
                        Age,
                        GroupId
                    FROM dbo.Students
                    ORDER BY Id";

                return connection.Query<Student>(sql).ToList();
            }
        }

        // GET STUDENT BY ID
        public Student GetStudentById(int id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = @"
                    SELECT
                        Id AS StudentId,
                        FirstName,
                        LastName,
                        Age,
                        GroupId
                    FROM dbo.Students
                    WHERE Id = @id";

                return connection.QueryFirstOrDefault<Student>(
                    sql,
                    new { id }
                );
            }
        }

        // CREATE STUDENT
        public int CreateStudent(Student student)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = @"
                    INSERT INTO dbo.Students
                        (FirstName, LastName, Age, GroupId)
                    OUTPUT INSERTED.Id
                    VALUES
                        (@FirstName, @LastName, @Age, @GroupId)";

                return connection.QuerySingle<int>(sql, student);
            }
        }

        // UPDATE STUDENT
        public bool UpdateStudent(int id, Student student)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = @"
                    UPDATE dbo.Students
                    SET
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Age = @Age,
                        GroupId = @GroupId
                    WHERE Id = @Id";

                int rowsAffected = connection.Execute(sql, new
                {
                    Id = id,
                    student.FirstName,
                    student.LastName,
                    student.Age,
                    student.GroupId
                });

                return rowsAffected > 0;
            }
        }

        // DELETE STUDENT
        public bool DeleteStudent(int id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                string sql = @"
                    DELETE FROM dbo.Students
                    WHERE Id = @Id";

                int rowsAffected = connection.Execute(
                    sql,
                    new { Id = id }
                );

                return rowsAffected > 0;
            }
        }
    }
}
