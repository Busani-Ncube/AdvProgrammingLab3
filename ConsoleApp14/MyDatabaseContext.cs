using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

public class MyDatabaseContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use a fixed path that's easy to locate and verify
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "school.db");
        Console.WriteLine($"Database path: {dbPath}");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-Many: Teacher has many Classes, Class belongs to one Teacher
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany(t => t.Classes)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // Many-to-Many: Students can be in many Classes, Classes can have many Students
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Classes)
            .WithMany(c => c.Students);

        base.OnModelCreating(modelBuilder);
    }
}
