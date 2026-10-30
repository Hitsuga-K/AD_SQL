using College_DB.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    public class GroupRepository
    {
        private readonly string _conn_str;

        public GroupRepository(string connectionString)
        {
            _conn_str = connectionString;
        }

        public List<Group> GetAllGroups()
        {
            using var connection = new SqlConnection(_conn_str);
            return connection
                .Query<Group>("SELECT GroupId, GroupName FROM Groups ORDER BY GroupId")
                .ToList();
        }

        public int CreateGroup(string groupName)
        {
            using var connection = new SqlConnection(_conn_str);
            return connection
                .QuerySingle<int>("INSERT INTO Groups (GroupName) OUTPUT INSERTED.GroupId VALUES (@GroupName)",
                new { GroupName = groupName });
        }

        public Group GetGroupById(int id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection
                    .QueryFirstOrDefault<Group>(
                        "SELECT GroupId, GroupName FROM Groups WHERE GroupId = @id",
                        new { id });
            }
        }
        public bool UpdateGroup(int id, string groupName)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                "UPDATE Groups SET GroupName = @GroupName WHERE GroupId = @id",
                new { id, GroupName = groupName }) > 0;
        }
        public bool DeleteGroup(int id)
        {
            using var connection = new SqlConnection(_conn_str);

            return connection.Execute(
                "DELETE FROM Groups WHERE GroupId = @id",
                new { id }) > 0;
        }
    }
}