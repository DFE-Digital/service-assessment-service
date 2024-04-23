using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ServiceAssessmentService.Application.Database
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // // Determine the base path for the configuration file
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ServiceAssessmentService.WebApp");

            // Build the configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.development.json")
                .Build();

            // Initialize the connection string from the configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Check if connection string is null or empty
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is null or empty.");
            }

            // Setup DbContext options with SQL Server provider
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create and return the DbContext instance
            return new DataContext(optionsBuilder.Options);
        }
    }
}
