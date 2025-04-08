using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using RealEstate_Api.Models;

namespace RealEstate_Api.Data
{
    public class AgentRepository
    {
        private readonly string _connectionString;

        public AgentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public IEnumerable<AgentModel> SelectAllAgents()
        {
            var agents = new List<AgentModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Agent_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    agents.Add(new AgentModel
                    {
                        AgentId = Convert.ToInt32(reader["AgentId"]),
                        AgentName = reader["AgentName"].ToString(),
                        LicenseNumber = reader["LicenseNumber"].ToString(),
                        ExperienceYears = Convert.ToInt32(reader["ExperienceYears"]),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        OfficeAddress = reader["OfficeAddress"].ToString(),
                        ProfilePicture = reader["ProfilePicture"].ToString(),
                        ProfilePicturePath = reader["ProfilePicturePath"].ToString(),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return agents;
        }
        #endregion

        #region SelectById
        public AgentModel GetAgentById(int agentId)
        {
            AgentModel agent = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Agent_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AgentId", agentId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    agent = new AgentModel
                    {
                        AgentId = Convert.ToInt32(reader["AgentId"]),
                        AgentName = reader["AgentName"].ToString(),
                        LicenseNumber = reader["LicenseNumber"].ToString(),
                        ExperienceYears = Convert.ToInt32(reader["ExperienceYears"]),
                        ContactNumber = reader["ContactNumber"].ToString(),
                        OfficeAddress = reader["OfficeAddress"].ToString(),
                        ProfilePicture = reader["ProfilePicture"].ToString(),
                        ProfilePicturePath = reader["ProfilePicturePath"].ToString(),       
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return agent;
        }
        #endregion

        #region Insert
        public bool InsertAgent(AgentModel agent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Agent_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AgentName", agent.AgentName);
                cmd.Parameters.AddWithValue("@LicenseNumber", agent.LicenseNumber);
                cmd.Parameters.AddWithValue("@ExperienceYears", agent.ExperienceYears);
                cmd.Parameters.AddWithValue("@ContactNumber", agent.ContactNumber);
                cmd.Parameters.AddWithValue("@OfficeAddress", agent.OfficeAddress ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProfilePicture", agent.ProfilePicture ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProfilePicturePath", agent.ProfilePicturePath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", agent.Status);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Update
        public bool UpdateAgent(AgentModel agent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Agent_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AgentId", agent.AgentId);
                cmd.Parameters.AddWithValue("@AgentName", agent.AgentName);
                cmd.Parameters.AddWithValue("@LicenseNumber", agent.LicenseNumber);
                cmd.Parameters.AddWithValue("@ExperienceYears", agent.ExperienceYears);
                cmd.Parameters.AddWithValue("@ContactNumber", agent.ContactNumber);
                cmd.Parameters.AddWithValue("@OfficeAddress", agent.OfficeAddress ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProfilePicture", agent.ProfilePicture ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProfilePicturePath", agent.ProfilePicturePath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", agent.Status);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Delete
        public bool DeleteAgent(int agentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Agent_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AgentId", agentId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region AgentsByStatus
        public List<AgentModel> GetAgentsByStatus(string status)
        {
            var agents = new List<AgentModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_GetAgentsByStatus", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Status", status);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            agents.Add(new AgentModel
                            {
                                AgentId = Convert.ToInt32(reader["AgentId"]),
                                AgentName = reader["AgentName"].ToString(),
                                LicenseNumber = reader["LicenseNumber"].ToString(),
                                ExperienceYears = Convert.ToInt32(reader["ExperienceYears"]),
                                ContactNumber = reader["ContactNumber"].ToString(),
                                OfficeAddress = reader["OfficeAddress"].ToString(),
                                ProfilePicture = reader["ProfilePicture"].ToString(),
                                ProfilePicturePath = reader["ProfilePicturePath"].ToString(),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }
            return agents;
        }
        #endregion
    }
}
