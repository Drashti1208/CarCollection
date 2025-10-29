using System.ComponentModel.DataAnnotations;

namespace CarCollection.Web.Models
{
    public class ServiceRecord
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        [DataType(DataType.Date)] public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = "";
        public int? Mileage { get; set; }
        public decimal? Cost { get; set; }

        public Car? Car { get; set; }
    }
}
