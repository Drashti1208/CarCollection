using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarCollection.Web.Data
{
    // This factory provides a way for the EF tools to create the DbContext at design-time
    // (for example when creating migrations) so that content root / Startup project
    // discovery issues don't block `dotnet ef`.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Use the same SQLite file as configured in appsettings.json
            optionsBuilder.UseSqlite("Data Source=carcollection.db");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
