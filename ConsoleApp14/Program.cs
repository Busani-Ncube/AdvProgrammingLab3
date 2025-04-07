class Program
{
    static void Main(string[] args)
    {
        using (var db = new MyDatabaseContext())
        {
            // Clear old data
            db.Students.RemoveRange(db.Students);
            db.Classes.RemoveRange(db.Classes);
            db.SaveChanges();

            // Add new data
            var student1 = new Student { FirstName = "John", LastName = "Doe" };
            var student2 = new Student { FirstName = "Jane", LastName = "Smith" };
            var class1 = new Class { Name = "Math" };
            var class2 = new Class { Name = "History" };

            db.Students.AddRange(student1, student2);
            db.Classes.AddRange(class1, class2);
            db.SaveChanges();

            // Print results
            PrintStudents(db);
            PrintClasses(db);
        }
    }

    static void PrintStudents(MyDatabaseContext db)
    {
        Console.WriteLine("Students:");
        foreach (var student in db.Students.ToList())
        {
            Console.WriteLine(student);
        }
    }

    static void PrintClasses(MyDatabaseContext db)
    {
        Console.WriteLine("\nClasses:");
        foreach (var cls in db.Classes.ToList())
        {
            Console.WriteLine(cls);
        }
    }
}




