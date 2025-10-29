using Microsoft.EntityFrameworkCore;
using CarCollection.Web.Models;

namespace CarCollection.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<CarPhoto> CarPhotos { get; set; } = null!;
        public DbSet<ServiceRecord> ServiceRecords { get; set; } = null!;
    }
}
