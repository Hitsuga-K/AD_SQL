using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    public class SubjectRepository
    {
        private readonly string _conn_str;

        public SubjectRepository(string connectionString)
        {
            _conn_str = connectionString;
        }

        public List<Subject> GetAllSubjects()
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Query<Subject>(
                "SELECT Id, SubjectName, TeacherId FROM Subjects")
                .ToList();
        }

        public Subject GetSubjectById(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.QueryFirstOrDefault<Subject>(
                "SELECT Id, SubjectName, TeacherId FROM Subjects WHERE Id = @id",
                new { id });
        }

        public int CreateSubject(Subject subject)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.QuerySingle<int>(
                @"INSERT INTO Subjects (SubjectName, TeacherId)
                  OUTPUT INSERTED.Id
                  VALUES (@SubjectName, @TeacherId)",
                subject);
        }

        public bool UpdateSubject(int id, Subject subject)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                @"UPDATE Subjects
                  SET SubjectName = @SubjectName,
                      TeacherId = @TeacherId
                  WHERE Id = @Id",
                new
                {
                    subject.SubjectName,
                    subject.TeacherId,
                    Id = id
                }) > 0;
        }

        public bool DeleteSubject(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                "DELETE FROM Subjects WHERE Id = @id",
                new { id }) > 0;
        }
    }
}