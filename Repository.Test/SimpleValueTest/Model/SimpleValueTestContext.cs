using Microsoft.EntityFrameworkCore;
using System;

namespace Repository.Test;
public class SimpleValueTestContext : DbContext
{
    public DbSet<SimpleValueTestModel> TestModel { get; set; }

    public SimpleValueTestContext(DbContextOptions<SimpleValueTestContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimpleValueTestModel>().ToTable("SimpleValueTestModel");
        modelBuilder.Entity<SimpleValueTestModel>().HasNoKey();
    }
}
