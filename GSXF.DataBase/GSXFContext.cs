using System.Data.Entity;
using GSXF.Model;

namespace GSXF.DataBase
{
    public class GSXFContext:DbContext
    {

        public GSXFContext() : base("DefaultConnection")
        {
            Database.SetInitializer<GSXFContext>(new CreateDatabaseIfNotExists<GSXFContext>());
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Employee> Employees { get; set; }


        public DbSet<Project> Projects { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserCompany> UserCompanies { get; set; }

        public DbSet<FireControlInstitution> FireControlInstitutions { get; set; }

        public DbSet<UploadFile> UploadFiles { get; set; }


        public DbSet<Evaluation> Evaluations { get; set; }
    } 
}
