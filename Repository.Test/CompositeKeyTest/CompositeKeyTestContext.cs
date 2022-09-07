using Microsoft.EntityFrameworkCore;
using Test.Model;

namespace Repository.Test;
public class CompositeKeyTestContext : DbContext
{
    public DbSet<CompositeKeyModel> TestModel { get; set; }

    public CompositeKeyTestContext(DbContextOptions<CompositeKeyTestContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompositeKeyModel>().ToTable("CompositeKeyTestModel");
        modelBuilder.Entity<CompositeKeyModel>().HasKey(x => new { x.Id, x.Date });
        modelBuilder.Entity<CompositeKeyModel>().Property(x => x.Id).ValueGeneratedNever();
    }
}
