using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using RealEstate_Api.Models;
using Microsoft.Extensions.Configuration;

namespace RealEstate_Api.Data
{
    public class PropertyTypeRepository
    {
        private readonly string _connectionString;

        public PropertyTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region GetAll
        public IEnumerable<PropertyTypeModel> GetAllPropertyTypes()
        {
            var propertyTypes = new List<PropertyTypeModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_PropertyType_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    propertyTypes.Add(new PropertyTypeModel
                    {
                        PropertyTypeId = Convert.ToInt32(reader["PropertyTypeId"]),
                        PropertyType = reader["PropertyType"].ToString()
                    });
                }
            }
            return propertyTypes;
        }
        #endregion

        #region GetById
        public PropertyTypeModel GetPropertyTypeById(int propertyTypeId)
        {
            PropertyTypeModel propertyType = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_PropertyType_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyTypeId", propertyTypeId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    propertyType = new PropertyTypeModel
                    {
                        PropertyTypeId = Convert.ToInt32(reader["PropertyTypeId"]),
                        PropertyType = reader["PropertyType"].ToString()
                    };
                }
            }
            return propertyType;
        }
        #endregion

        #region Insert
        public bool InsertPropertyType(PropertyTypeModel propertyType)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_PropertyType_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyType", propertyType.PropertyType);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Update
        public bool UpdatePropertyType(PropertyTypeModel propertyType)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_PropertyType_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyTypeId", propertyType.PropertyTypeId);
                cmd.Parameters.AddWithValue("@PropertyType", propertyType.PropertyType);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Delete
        public bool DeletePropertyType(int propertyTypeId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_PropertyType_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyTypeId", propertyTypeId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion
    }
}
