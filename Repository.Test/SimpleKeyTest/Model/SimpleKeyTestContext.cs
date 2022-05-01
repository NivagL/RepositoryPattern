using Microsoft.EntityFrameworkCore;

namespace Repository.Test;
public class SimpleKeyTestContext : DbContext
{
    public DbSet<SimpleKeyTestModel> TestModel { get; set; }

    public SimpleKeyTestContext(DbContextOptions<SimpleKeyTestContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimpleKeyTestModel>().ToTable("SimpleKeyTestModel");
        modelBuilder.Entity<SimpleKeyTestModel>().HasKey(x => x.Id);
        modelBuilder.Entity<SimpleKeyTestModel>().Property(x => x.Id).ValueGeneratedNever();
    }
}
