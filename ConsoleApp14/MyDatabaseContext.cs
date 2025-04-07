using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

public class MyDatabaseContext : DbContext
{
    public string DbPath { get; }

    public MyDatabaseContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "blogging.db");
    }

    // Configure EF to use SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    // Declare tables for Student and Class (will be created later)
    public DbSet<Student> Students { get; set; }
    public DbSet<Class> Classes { get; set; }
}


