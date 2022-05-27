using Microsoft.EntityFrameworkCore;
using Repository.Test.Model;

namespace Repository.Test;
public class CompositeKeyTestContext : DbContext
{
    public DbSet<CompositeKeyTestModel> TestModel { get; set; }

    public CompositeKeyTestContext(DbContextOptions<CompositeKeyTestContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompositeKeyTestModel>().ToTable("CompositeKeyTestModel");
        modelBuilder.Entity<CompositeKeyTestModel>().HasKey(x => new { x.Id, x.Date });
        modelBuilder.Entity<CompositeKeyTestModel>().Property(x => x.Id).ValueGeneratedNever();
    }
}
