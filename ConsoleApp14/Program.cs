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

                // Clear the database first
                ClearDatabase(context);

                // Add a teacher first
                Console.WriteLine("\nAdding a teacher...");
                var teacher = new Teacher { FirstName = "John", LastName = "Doe" };
                context.Teachers.Add(teacher);
                context.SaveChanges();
                Console.WriteLine($"Added teacher: {teacher}");

                // Now add multiple classes and assign them to the teacher
                Console.WriteLine("\nAdding multiple classes to the teacher...");
                var mathClass = new Class { Name = "Mathematics", Teacher = teacher };
                var physicsClass = new Class { Name = "Physics", Teacher = teacher };
                var chemistryClass = new Class { Name = "Chemistry", Teacher = teacher };

                context.Classes.AddRange(mathClass, physicsClass, chemistryClass);
                context.SaveChanges();

                Console.WriteLine($"Added classes: {mathClass}, {physicsClass}, {chemistryClass}");

                // Retrieve the teacher with their classes
                Console.WriteLine("\nRetrieving teacher with classes...");
                var teacherWithClasses = context.Teachers
                    .Include(t => t.Classes)
                    .FirstOrDefault(t => t.Id == teacher.Id);

                Console.WriteLine($"Teacher: {teacherWithClasses.FirstName} {teacherWithClasses.LastName}");
                Console.WriteLine("Classes taught by this teacher:");
                foreach (var cls in teacherWithClasses.Classes)
                {
                    Console.WriteLine($"  - {cls.Name} (ID: {cls.ClassId})");
                }

                // Add another teacher with different classes
                Console.WriteLine("\nAdding another teacher with different classes...");
                var anotherTeacher = new Teacher { FirstName = "Jane", LastName = "Smith" };
                var biologyClass = new Class { Name = "Biology", Teacher = anotherTeacher };
                var historyClass = new Class { Name = "History", Teacher = anotherTeacher };

                context.Teachers.Add(anotherTeacher);
                context.Classes.AddRange(biologyClass, historyClass);
                context.SaveChanges();

                // Print all teachers and their classes
                Console.WriteLine("\nAll teachers and their classes:");
                PrintAllTeachersWithClasses(context);

                // Remove a class from a teacher
                Console.WriteLine("\nRemoving a class from a teacher...");
                var classToRemove = context.Classes.First(c => c.Name == "Mathematics");
                context.Classes.Remove(classToRemove);
                context.SaveChanges();

                Console.WriteLine("\nTeachers and classes after removal:");
                PrintAllTeachersWithClasses(context);

                // QUESTION 4: Many-to-Many relationship between Students and Classes
                Console.WriteLine("\n--- QUESTION 4: Many-to-Many Relationship ---");

                // Add students
                Console.WriteLine("\nAdding students...");
                var student1 = new Student { FirstName = "Alice", LastName = "Johnson" };
                var student2 = new Student { FirstName = "Bob", LastName = "Smith" };
                var student3 = new Student { FirstName = "Charlie", LastName = "Brown" };

                context.Students.AddRange(student1, student2, student3);
                context.SaveChanges();
                Console.WriteLine($"Added students: {student1}, {student2}, {student3}");

                // Enroll students in classes
                Console.WriteLine("\nEnrolling students in classes...");

                // Get classes
                var physicsClassDb = context.Classes.First(c => c.Name == "Physics");
                var chemistryClassDb = context.Classes.First(c => c.Name == "Chemistry");
                var biologyClassDb = context.Classes.First(c => c.Name == "Biology");
                var historyClassDb = context.Classes.First(c => c.Name == "History");

                // Alice takes Physics and Biology
                student1.Classes.Add(physicsClassDb);
                student1.Classes.Add(biologyClassDb);

                // Bob takes Chemistry and History
                student2.Classes.Add(chemistryClassDb);
                student2.Classes.Add(historyClassDb);

                // Charlie takes all classes
                student3.Classes.Add(physicsClassDb);
                student3.Classes.Add(chemistryClassDb);
                student3.Classes.Add(biologyClassDb);
                student3.Classes.Add(historyClassDb);

                context.SaveChanges();
                Console.WriteLine("Students enrolled in classes");

                // Print students and their classes
                Console.WriteLine("\nAll students and their classes:");
                PrintAllStudentsWithClasses(context);

                // Print classes and their students
                Console.WriteLine("\nAll classes and their students:");
                PrintAllClassesWithStudents(context);

                // Remove a student from a class
                Console.WriteLine("\nRemoving a student from a class...");
                var studentToModify = context.Students
                    .Include(s => s.Classes)
                    .First(s => s.FirstName == "Charlie");

                var classToRemoveFrom = studentToModify.Classes
                    .First(c => c.Name == "Physics");

                studentToModify.Classes.Remove(classToRemoveFrom);
                context.SaveChanges();

                Console.WriteLine("Updated students and their classes:");
                PrintAllStudentsWithClasses(context);

                Console.WriteLine("\nUpdated classes and their students:");
                PrintAllClassesWithStudents(context);
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

    static void ClearDatabase(MyDatabaseContext context)
    {
        Console.WriteLine("\nClearing database...");

        // Important: Clear join table entries first to avoid foreign key constraints
        foreach (var student in context.Students.Include(s => s.Classes).ToList())
        {
            student.Classes.Clear();
        }
        context.SaveChanges();

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
        if (context.Students.Any())
        {
            context.Students.RemoveRange(context.Students);
            context.SaveChanges();
            Console.WriteLine("Students cleared");
        }
    }

    static void PrintAllTeachersWithClasses(MyDatabaseContext context)
    {
        var teachers = context.Teachers.Include(t => t.Classes).ToList();
        foreach (var t in teachers)
        {
            Console.WriteLine($"Teacher: {t.FirstName} {t.LastName} (ID: {t.Id})");
            if (t.Classes.Any())
            {
                Console.WriteLine("  Classes:");
                foreach (var c in t.Classes)
                {
                    Console.WriteLine($"    - {c.Name} (ID: {c.ClassId})");
                }
            }
            else
            {
                Console.WriteLine("  No classes assigned");
            }
        }
    }

    static void PrintAllStudentsWithClasses(MyDatabaseContext context)
    {
        var students = context.Students.Include(s => s.Classes).ToList();
        foreach (var s in students)
        {
            Console.WriteLine($"Student: {s.FirstName} {s.LastName} (ID: {s.Id})");
            if (s.Classes.Any())
            {
                Console.WriteLine("  Classes:");
                foreach (var c in s.Classes)
                {
                    Console.WriteLine($"    - {c.Name} (ID: {c.ClassId})");
                }
            }
            else
            {
                Console.WriteLine("  No classes enrolled");
            }
        }
    }

    static void PrintAllClassesWithStudents(MyDatabaseContext context)
    {
        var classes = context.Classes
            .Include(c => c.Students)
            .Include(c => c.Teacher)
            .ToList();

        foreach (var c in classes)
        {
            Console.WriteLine($"Class: {c.Name} (ID: {c.ClassId}), Teacher: {(c.Teacher != null ? c.Teacher.FirstName + " " + c.Teacher.LastName : "None")}");
            if (c.Students.Any())
            {
                Console.WriteLine("  Students:");
                foreach (var s in c.Students)
                {
                    Console.WriteLine($"    - {s.FirstName} {s.LastName} (ID: {s.Id})");
                }
            }
            else
            {
                Console.WriteLine("  No students enrolled");
            }
        }
    }
}