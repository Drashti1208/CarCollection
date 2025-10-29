using System.ComponentModel.DataAnnotations;

namespace CarCollection.Web.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required] public string Make { get; set; } = "";
        [Required] public string Model { get; set; } = "";
        [Range(1886, 2100)] public int Year { get; set; }
        public string? VIN { get; set; }
        public string? Color { get; set; }
        public int? Mileage { get; set; }

        [DataType(DataType.Date)] public DateTime? PurchaseDate { get; set; }
        public string? Notes { get; set; }

        // Navigation
        public List<CarPhoto>? Photos { get; set; }
        public List<ServiceRecord>? ServiceRecords { get; set; }
    }
}
