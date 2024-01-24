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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<Entities.AssessmentRequest>()
            .ToTable("AssessmentRequests", b => b.IsTemporal());
    }

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
