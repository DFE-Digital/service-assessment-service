using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceAssessmentService.Application.Database.Entities;

namespace ServiceAssessmentService.Application.Database;

public class DataContext : IdentityDbContext<ServiceAssessmentServiceWebAppUser>
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    internal DbSet<Entities.AssessmentRequest> AssessmentRequests { get; set; } = null!;
    internal DbSet<Entities.Phase> Phases { get; set; } = null!;
    internal DbSet<Entities.AssessmentType> AssessmentTypes { get; set; } = null!;
    public DbSet<Entities.Person> People { get; set; } = null!;
    internal DbSet<Entities.Portfolio> Portfolios { get; set; } = null!;


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update/set timestamps where the entity is derived from BaseEntity (all database entities should extend this)
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is
            {
                Entity: BaseEntity,
                State: EntityState.Added or EntityState.Modified,
            }
            );

        foreach (var entityEntry in entries)
        {
            // Always update the updated time
            ((BaseEntity)entityEntry.Entity).UpdatedUtc = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                // Only set created timestamp if entity is newly added
                ((BaseEntity)entityEntry.Entity).CreatedUtc = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
