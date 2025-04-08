using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RealEstate_Api.Models;
using System.Data;

namespace RealEstate_Api.Data
{
    public class AppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAll
        [HttpGet]
        public IEnumerable<AppointmentModel> GetAllAppointments()
        {
            var appointments = new List<AppointmentModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    appointments.Add(new AppointmentModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UserEmail = reader["UserEmail"].ToString(),
                        AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return appointments;
        }
        #endregion

        #region statusupdate
        public bool UpdateStatus(int id, string status)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_UpdateStatus", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Status", status);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0; 
            }
        }
        #endregion

        #region Insert
        public bool InsertAppointment(AppointmentModel appointment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Appointment_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserEmail", appointment.UserEmail);
                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@Status", appointment.Status ?? "Pending");

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region GetByID
        public AppointmentModel GetAppointmentById(int id)
        {
            AppointmentModel appointment = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    appointment = new AppointmentModel
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        UserEmail = reader["UserEmail"].ToString(),
                        AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                        Status = reader["Status"].ToString()
                    };
                }
            }

            return appointment;
        }
        #endregion
    }
}
