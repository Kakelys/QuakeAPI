using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuakeAPI.Data
{
    /// <summary>
    /// used by EF to create migration (imitation DI for db context)
    /// </summary>
    public class QuakeDbContextFactory : IDesignTimeDbContextFactory<QuakeDbContext>
    {
        public QuakeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<QuakeDbContext>();

            string workingDirectory = Environment.CurrentDirectory;

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile($"{workingDirectory}\\appsettings.json");
            IConfigurationRoot config = builder.Build();

            string? connectionString = config.GetConnectionString("QuakeDb");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds));

            return new QuakeDbContext(optionsBuilder.Options);
        }
    }
}