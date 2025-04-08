using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using RealEstate_Api.Models;

namespace RealEstate_Api.Data
{
    public class PropertyRepository
    {
        private readonly string _connectionString;

        public PropertyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        #region SelectAll
        public IEnumerable<PropertyModel> SelectAllProperties()
        {
            var properties = new List<PropertyModel>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Property_SelectAll", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    properties.Add(new PropertyModel
                    {
                        PropertyId = Convert.ToInt32(reader["PropertyId"]),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        PropertyType = reader["PropertyType"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Location = reader["Location"].ToString(),
                        Area = Convert.ToSingle(reader["Area"]),
                        Bedrooms = reader["Bedrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bedrooms"]) : (int?)null,
                        Bathrooms = reader["Bathrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bathrooms"]) : (int?)null,
                        Amenities = reader["Amenities"].ToString(),
                        ListedDate = Convert.ToDateTime(reader["ListedDate"]),
                        Status = reader["Status"].ToString(),
                        ImageName = reader["ImageName"].ToString(),
                        ImagePath = reader["ImagePath"].ToString()
                    });
                }
            }
            return properties;
        }
        #endregion

        #region SelectById
        public PropertyModel GetPropertyById(int propertyId)
        {
            PropertyModel property = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Property_SelectByPK", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyId", propertyId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    property = new PropertyModel
                    {
                        PropertyId = Convert.ToInt32(reader["PropertyId"]),
                        Title = reader["Title"].ToString(),
                        Description = reader["Description"].ToString(),
                        PropertyType = reader["PropertyType"].ToString(),
                        Category = reader["Category"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Location = reader["Location"].ToString(),
                        Area = Convert.ToSingle(reader["Area"]),
                        Bedrooms = reader["Bedrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bedrooms"]) : (int?)null,
                        Bathrooms = reader["Bathrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bathrooms"]) : (int?)null,
                        Amenities = reader["Amenities"].ToString(),
                        ListedDate = Convert.ToDateTime(reader["ListedDate"]),
                        Status = reader["Status"].ToString(),
                        ImageName = reader["ImageName"].ToString(),
                        ImagePath = reader["ImagePath"].ToString()
                    };
                }
            }
            return property;
        }
        #endregion

        #region Insert
        public bool InsertProperty(PropertyModel property)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Property_Insert", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Title", property.Title);
                cmd.Parameters.AddWithValue("@Description", property.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PropertyType", property.PropertyType);
                cmd.Parameters.AddWithValue("@PropertyTypeId", property.PropertyTypeId);
                cmd.Parameters.AddWithValue("@Category", property.Category);
                cmd.Parameters.AddWithValue("@Price", property.Price);
                cmd.Parameters.AddWithValue("@Location", property.Location);
                cmd.Parameters.AddWithValue("@Area", property.Area);
                cmd.Parameters.AddWithValue("@Bedrooms", property.Bedrooms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Bathrooms", property.Bathrooms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Amenities", property.Amenities ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", property.Status);
                cmd.Parameters.AddWithValue("@ImageName", property.ImageName);
                cmd.Parameters.AddWithValue("@ImagePath", property.ImagePath);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Update
        public bool UpdateProperty(PropertyModel property)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Property_Update", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyId", property.PropertyId);
                cmd.Parameters.AddWithValue("@Title", property.Title);
                cmd.Parameters.AddWithValue("@Description", property.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PropertyTypeId", property.PropertyTypeId);
                cmd.Parameters.AddWithValue("@Category", property.Category);
                cmd.Parameters.AddWithValue("@Price", property.Price);
                cmd.Parameters.AddWithValue("@Location", property.Location);
                cmd.Parameters.AddWithValue("@Area", property.Area);
                cmd.Parameters.AddWithValue("@Bedrooms", property.Bedrooms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Bathrooms", property.Bathrooms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Amenities", property.Amenities ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", property.Status);
                cmd.Parameters.AddWithValue("@ImageName", property.ImageName);
                cmd.Parameters.AddWithValue("@ImagePath", property.ImagePath);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region Delete
        public bool DeleteProperty(int propertyId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_Property_Delete", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@PropertyId", propertyId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        #endregion

        #region PropertyByCategory
        public List<PropertyModel> GetPropertiesByCategory(string category)
        {
            var properties = new List<PropertyModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_GetPropertiesForCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Category", category);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            properties.Add(new PropertyModel
                            {
                                Title = reader["Title"]?.ToString(),
                                Description = reader["Description"]?.ToString(),
                                PropertyType = reader["PropertyType"]?.ToString(),
                                Category = reader["Category"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Location = reader["Location"]?.ToString(),
                                Area = Convert.ToSingle(reader["Area"]),
                                Bedrooms = reader["Bedrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bedrooms"]) : (int?)null,
                                Bathrooms = reader["Bathrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bathrooms"]) : (int?)null,
                                Amenities = reader["Amenities"]?.ToString(),
                                Status = reader["Status"]?.ToString(),
                                ImageName = reader["ImageName"]?.ToString(),
                                ImagePath = reader["ImagePath"]?.ToString()
                            });
                        }
                    }
                }
            }

            return properties;
        }
        #endregion

        #region PropertyByType
        public List<PropertyModel> GetPropertiesByPropertyType(string propertyType)
        {
            var properties = new List<PropertyModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("PR_GetPropertiesByType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PropertyType", propertyType);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            properties.Add(new PropertyModel
                            {
                                Title = reader["Title"]?.ToString(),
                                Description = reader["Description"]?.ToString(),
                                PropertyType = reader["PropertyType"]?.ToString(),
                                Category = reader["Category"]?.ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Location = reader["Location"]?.ToString(),
                                Area = Convert.ToSingle(reader["Area"]),
                                Bedrooms = reader["Bedrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bedrooms"]) : (int?)null,
                                Bathrooms = reader["Bathrooms"] != DBNull.Value ? Convert.ToInt32(reader["Bathrooms"]) : (int?)null,
                                Amenities = reader["Amenities"]?.ToString(),
                                Status = reader["Status"]?.ToString(),
                                ImageName = reader["ImageName"]?.ToString(),
                                ImagePath = reader["ImagePath"]?.ToString()
                            });
                        }
                    }
                }
            }

            return properties;
        }
        #endregion


    }
}
