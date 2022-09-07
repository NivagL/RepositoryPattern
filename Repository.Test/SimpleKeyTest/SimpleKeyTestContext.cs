using Microsoft.EntityFrameworkCore;
using Test.Model;

namespace Repository.Test;
public class SimpleKeyTestContext : DbContext
{
    public DbSet<SimpleGuidModel> TestModel { get; set; }

    public SimpleKeyTestContext(DbContextOptions<SimpleKeyTestContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimpleGuidModel>().ToTable("SimpleKeyTestModel");
        modelBuilder.Entity<SimpleGuidModel>().HasKey(x => x.Id);
        modelBuilder.Entity<SimpleGuidModel>().Property(x => x.Id).ValueGeneratedNever();
    }
}
