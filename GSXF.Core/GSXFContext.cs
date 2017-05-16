using System.Data.Entity;

namespace GSXF.Core
{
    public class GSXFContext:DbContext
    {

        public GSXFContext() : base("DefaultConnection")
        {
            Database.SetInitializer<GSXFContext>(new CreateDatabaseIfNotExists<GSXFContext>());
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<OfficeAddress> OfficeAddresses { get; set; }

        public DbSet<Project> Projects { get; set; }
    }
}
