using System.Reflection;

namespace RealEstate_Admin.Models
{
    public class HomeModel
    {
        public int TotalUsers { get; set; }
        public decimal TotalProperties { get; set; }
        public int TotalAgents { get; set; }
        public int TotalSellProperties { get; set; }
        public int TotalRentProperties { get; set; }
        public int TotalBuyProperties { get; set; }
        public int TotalActiveAgents { get; set; }
        public int TotalInactiveAgents { get; set; }
        public int TotalSuspendedAgents { get; set; }
        public List<PropertiesModel> RecentPropertySold { get; set; } = new List<PropertiesModel>();

    }

    public class PropertiesModel
    {
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PropertyType { get; set; }
        public int PropertyTypeId { get; set; } 
        public string Category { get; set; } 
        public decimal Price { get; set; }
        public string Location { get; set; }
        public float Area { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string Amenities { get; set; }
        public DateTime ListedDate { get; set; }
        public string Status { get; set; } 
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    }
}
