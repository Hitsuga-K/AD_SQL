using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    public class GradeRepository
    {
        private readonly string _conn_str;

        public GradeRepository(string connectionString)
        {
            _conn_str = connectionString;
        }

        public List<Grade> GetAllGrades()
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Query<Grade>(
                @"SELECT Id, StudentId, SubjectId,
                  TeacherId, GradeValue, GradeDate
                  FROM Grades").ToList();
        }

        public Grade GetGradeById(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.QueryFirstOrDefault<Grade>(
                @"SELECT Id, StudentId, SubjectId,
                  TeacherId, GradeValue, GradeDate
                  FROM Grades WHERE Id = @id",
                new { id });
        }

        public int CreateGrade(Grade grade)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.QuerySingle<int>(
                @"INSERT INTO Grades
                  (StudentId, SubjectId, TeacherId, GradeValue, GradeDate)
                  OUTPUT INSERTED.Id
                  VALUES
                  (@StudentId, @SubjectId, @TeacherId, @GradeValue, @GradeDate)",
                grade);
        }

        public bool UpdateGrade(int id, Grade grade)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                @"UPDATE Grades
                  SET StudentId = @StudentId,
                      SubjectId = @SubjectId,
                      TeacherId = @TeacherId,
                      GradeValue = @GradeValue,
                      GradeDate = @GradeDate
                  WHERE Id = @Id",
                new
                {
                    grade.StudentId,
                    grade.SubjectId,
                    grade.TeacherId,
                    grade.GradeValue,
                    grade.GradeDate,
                    Id = id
                }) > 0;
        }

        public bool DeleteGrade(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                "DELETE FROM Grades WHERE Id = @id",
                new { id }) > 0;
        }
    }
}