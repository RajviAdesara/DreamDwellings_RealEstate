using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using RealEstate_Api.Models;
using Microsoft.EntityFrameworkCore.Metadata;
namespace RealEstate_Api.Data
{
    public class ContactUsRepository
    {

        private readonly string _connectionString;

        public ContactUsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAll
        [HttpGet]
        public IEnumerable<ContactUsModel> SelectAllFeedback()
        {
            var feedbacks = new List<ContactUsModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Feedback_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    feedbacks.Add(new ContactUsModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Message = reader["Message"].ToString()
                    });
                }
            }
            return feedbacks;
        }
        #endregion

        #region Insert
        public bool InsertFeedback(ContactUsModel feedback)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Feedback_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Name", feedback.Name);
                cmd.Parameters.AddWithValue("@Email", feedback.Email);
                cmd.Parameters.AddWithValue("@Message", feedback.Message);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion


    }
}
