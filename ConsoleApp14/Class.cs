public class Class
{
    public int ClassId { get; set; }
    public string Name { get; set; }

    // Foreign Key
    public int? TeacherId { get; set; }

    // Navigation Property
    public Teacher Teacher { get; set; }

    public override string ToString()
    {
        return $"Class: {ClassId}, {Name}, Teacher: {(Teacher != null ? Teacher.FirstName + " " + Teacher.LastName : "None")}";
    }
}
