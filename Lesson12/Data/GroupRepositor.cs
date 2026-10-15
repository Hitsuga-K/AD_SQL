using System.Collections.Generic;
using System.Linq;
using Dapper;
using College_DB.Models;
using Microsoft.Data.SqlClient;

namespace College_DB.Data
{
    class GroupRepository
    {
        private readonly string _conn_str;

        public GroupRepository(string connection_str)
        {
            _conn_str = connection_str;
        }

        public List<Group> GetAllGroups()
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection.Query<Group>(
                    "SELECT GroupId, GroupName FROM Groups"
                ).ToList();
            }
        }

        public Group GetGroupById(int id)
        {
            using (var connection = new SqlConnection(_conn_str))
            {
                return connection.QueryFirstOrDefault<Group>(
                    "SELECT GroupId, GroupName FROM Groups WHERE GroupId = @Id",
                    new { @Id = id }
                );
            }
        }
    }
}