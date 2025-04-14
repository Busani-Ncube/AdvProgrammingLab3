public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Many-to-many relationship with Class
    public ICollection<Class> Classes { get; set; } = new List<Class>();

    public override string ToString()
    {
        return $"Student: {Id}, {FirstName} {LastName}";
    }
}