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
        // One-to-One: Class has one Teacher, Teacher has one Class
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithOne(t => t.Class)
            .HasForeignKey<Class>(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull); // Optional: avoids cascade delete

        base.OnModelCreating(modelBuilder);
    }
}

