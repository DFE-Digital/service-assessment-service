﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ServiceAssessmentService.Application.Database;

/// <summary>
/// This class is used by the EF Core tools to create a DbContext for migrations.
///
/// If we do not use this class, the initialisation of this class (e.g., via another project's Program.cs) is required to run migrations.
///
/// See also:
/// https://learn.microsoft.com/en-gb/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    private const string ConnectionString = "Server=localhost;Database=ServiceAssessmentPlus-local;Trusted_Connection=True;TrustServerCertificate=True;";
    // private const string ConnectionString = "Server=serviceassessmentplus-dev.database.windows.net;Database=ServiceAssessmentPlus-dev;Authentication=Active Directory Integrated;";

    public DataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(ConnectionString);

        return new DataContext(optionsBuilder.Options);
    }
}
