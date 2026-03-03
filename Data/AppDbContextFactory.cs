using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagement.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// IConfiguration Property
        /// </summary>
        //private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        //public AppDbContextFactory(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public AppDbContext CreateDbContext(string[] args)
        {
            //throw new Exception("Factory is running");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            //var connectionString = _configuration.GetConnectionString("connStr");
            var connectionString = "server=127.0.0.1;port=3306;database=TaskManagementDB;user=root;password=Y@sh2010;SslMode=None;AllowPublicKeyRetrieval=True;";


            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
