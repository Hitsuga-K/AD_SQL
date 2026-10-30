using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    public class TeacherRepository
    {
        private readonly string _conn_str;

        public TeacherRepository(string connectionString)
        {
            _conn_str = connectionString;
        }

        public List<Teacher> GetAllTeachers()
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Query<Teacher>(
                @"SELECT Id, FirstName, LastName, Age, SubjectId
                  FROM Teachers").ToList();
        }

        public Teacher GetTeacherById(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.QueryFirstOrDefault<Teacher>(
                @"SELECT Id, FirstName, LastName, Age, SubjectId
                  FROM Teachers WHERE Id = @id",
                new { id });
        }

        public int CreateTeacher(Teacher teacher)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.QuerySingle<int>(
                @"INSERT INTO Teachers
                  (FirstName, LastName, Age, SubjectId)
                  OUTPUT INSERTED.Id
                  VALUES (@FirstName, @LastName, @Age, @SubjectId)",
                teacher);
        }

        public bool UpdateTeacher(int id, Teacher teacher)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                @"UPDATE Teachers
                  SET FirstName = @FirstName,
                      LastName = @LastName,
                      Age = @Age,
                      SubjectId = @SubjectId
                  WHERE Id = @Id",
                new
                {
                    teacher.FirstName,
                    teacher.LastName,
                    teacher.Age,
                    teacher.SubjectId,
                    Id = id
                }) > 0;
        }

        public bool DeleteTeacher(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                "DELETE FROM Teachers WHERE Id = @id",
                new { id }) > 0;
        }
    }
}