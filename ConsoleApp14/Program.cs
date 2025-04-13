using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Program started");

            // Ensure database is created
            using (var context = new MyDatabaseContext())
            {
                context.Database.EnsureCreated();
                Console.WriteLine("Database created successfully");

                // Print initial state
                Console.WriteLine($"Initial state - Database has {context.Classes.Count()} classes and {context.Teachers.Count()} teachers");

                // Clear the database first
                Console.WriteLine("\nClearing database...");
                if (context.Classes.Any())
                {
                    context.Classes.RemoveRange(context.Classes);
                    context.SaveChanges();
                    Console.WriteLine("Classes cleared");
                }
                if (context.Teachers.Any())
                {
                    context.Teachers.RemoveRange(context.Teachers);
                    context.SaveChanges();
                    Console.WriteLine("Teachers cleared");
                }

                // Step 2-a: Add a class first
                Console.WriteLine("\nAdding a Mathematics class...");
                var mathClass = new Class { Name = "Mathematics" };
                context.Classes.Add(mathClass);
                context.SaveChanges();
                Console.WriteLine($"Added class: {mathClass}");

                // Now retrieve the class by name as required in task 2a
                var retrievedClass = context.Classes
                    .FirstOrDefault(c => c.Name == "Mathematics");

                Console.WriteLine($"Retrieved class by name: {retrievedClass}");

                // Create and assign a teacher to the class
                Console.WriteLine("\nCreating a teacher and assigning to class...");
                var teacher = new Teacher { FirstName = "John", LastName = "Doe" };
                retrievedClass.Teacher = teacher;  // Establish the relationship
                context.Teachers.Add(teacher);
                context.SaveChanges();
                Console.WriteLine($"Added teacher: {teacher}");

                // Retrieve the class again to confirm the teacher is assigned
                var updatedClass = context.Classes
                    .Include(c => c.Teacher)
                    .FirstOrDefault(c => c.Name == "Mathematics");

                Console.WriteLine($"Class with teacher assigned: {updatedClass}");

                // Step 2-b: Add another teacher and class
                Console.WriteLine("\nAdding another teacher and class...");
                var physicsClass = new Class { Name = "Physics" };
                var physicsTeacher = new Teacher { FirstName = "Jane", LastName = "Smith" };
                physicsClass.Teacher = physicsTeacher;

                context.Classes.Add(physicsClass);
                context.Teachers.Add(physicsTeacher);
                context.SaveChanges();
                Console.WriteLine($"Added physics class: {physicsClass}");
                Console.WriteLine($"Added physics teacher: {physicsTeacher}");

                // Print all classes with their teachers
                Console.WriteLine("\nAll classes with their teachers:");
                var allClasses = context.Classes.Include(c => c.Teacher).ToList();
                foreach (var cls in allClasses)
                {
                    Console.WriteLine(cls);
                }

                // Remove a teacher and print classes to see how foreign key works
                Console.WriteLine("\nRemoving the physics teacher...");
                context.Teachers.Remove(physicsTeacher);
                context.SaveChanges();

                Console.WriteLine("\nClasses after removing a teacher:");
                var classesAfterRemoval = context.Classes.Include(c => c.Teacher).ToList();
                foreach (var cls in classesAfterRemoval)
                {
                    Console.WriteLine(cls);
                }

                // Remove the remaining class and print teachers
                Console.WriteLine("\nRemoving the mathematics class...");
                context.Classes.RemoveRange(context.Classes);
                context.SaveChanges();

                Console.WriteLine("\nTeachers after removing all classes:");
                var remainingTeachers = context.Teachers.ToList();
                foreach (var t in remainingTeachers)
                {
                    Console.WriteLine(t);
                }
            }

            Console.WriteLine("\nProgram completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
        }
        finally
        {
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}