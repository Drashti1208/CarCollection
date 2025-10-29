namespace CarCollection.Web.Models
{
    public class CarPhoto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string FilePath { get; set; } = "";
        public bool IsPrimary { get; set; } = false;

        public Car? Car { get; set; }
    }
}
