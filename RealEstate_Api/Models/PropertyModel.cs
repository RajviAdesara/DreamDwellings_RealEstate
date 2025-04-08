namespace RealEstate_Api.Models
{
    public class PropertyModel
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
        public string ImagePath { get; set; }

        public string ImageName { get; set; }
    }
}
