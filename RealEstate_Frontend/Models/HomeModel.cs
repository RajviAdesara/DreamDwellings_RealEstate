using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class HomeModel
    {
        public string PropertyType { get; set; }
        public int TotalCount { get; set; }

        public List<HomeModel> PropertyTypes { get; set; }
        public List<PropertyModel> RecentProperties { get; set; }
    }
}
