School Database Management System

C# | Entity Framework Core | SQLite

Project Overview

This project demonstrates the design and implementation of a relational database system for managing a school environment using C# and Entity Framework Core. It models real-world relationships between students, teachers, and classes while applying best practices in database structure and data handling.

🧩 Features & Implementation
🧑‍🎓 Student–Class Relationship (Many-to-Many)

Implemented a many-to-many relationship allowing multiple students to enroll in multiple classes.

Demonstrates proper use of join tables and navigation properties in EF Core.

👩‍🏫 Teacher–Class Relationship (One-to-Many)

Designed a one-to-many relationship where a single teacher can teach multiple classes.

Ensures referential integrity and correct foreign key usage.

🏫 Class–Teacher Relationship (One-to-One)

Implemented a one-to-one relationship linking each class to a specific teacher.

Demonstrates advanced entity configuration using Entity Framework Core.

🔧 CRUD Operations

Developed clean logic for inserting, retrieving, and deleting records.

Console-based testing confirms relationships and database interactions work as expected.

🛠 Technologies Used

C#

Entity Framework Core

SQLite

.NET Console Application

Structuring a backend system to reflect real-world data relationships

