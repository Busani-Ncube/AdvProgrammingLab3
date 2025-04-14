public class Class
{
    public int ClassId { get; set; }
    public string Name { get; set; }

    // Foreign Key for Teacher
    public int? TeacherId { get; set; }

    // Navigation Property for Teacher
    public Teacher Teacher { get; set; }

    // Many-to-many relationship with Student
    public ICollection<Student> Students { get; set; } = new List<Student>();

    public override string ToString()
    {
        return $"Class: {ClassId}, {Name}, Teacher: {(Teacher != null ? Teacher.FirstName + " " + Teacher.LastName : "None")}";
    }
}
