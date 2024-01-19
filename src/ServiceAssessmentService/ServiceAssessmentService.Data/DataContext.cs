using Microsoft.EntityFrameworkCore;
using ServiceAssessmentService.Data.Entities;

namespace ServiceAssessmentService.Data;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Entities.AssessmentRequest> AssessmentRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder
            .Entity<Entities.AssessmentRequest>()
            .ToTable("AssessmentRequests", builder => builder.IsTemporal());
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
            ((BaseEntity)entityEntry.Entity).Updated = DateTimeOffset.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                // Only set created timestamp if entity is newly added
                ((BaseEntity)entityEntry.Entity).Created = DateTimeOffset.UtcNow;
            }
            
            if (entityEntry.State == EntityState.Deleted)
            {
                // Only set deleted timestamp if entity is being deleted
                ((BaseEntity)entityEntry.Entity).Deleted = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
