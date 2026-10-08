using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var groups = new List<Group>();
            using (SqlConnection connection = new SqlConnection(_conn_str))
            {
                connection.Open();
                string sql = "SELECT GroupId, GroupName FROM Groups";
                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group
                        {
                            GroupId = reader.GetInt32(0),
                            GroupName = reader.GetString(1)
                        });
                    }
                }
            }
            return groups;
        }
    }
}