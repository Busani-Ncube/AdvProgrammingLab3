public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Navigation Property for one-to-many
    public ICollection<Class> Classes { get; set; } = new List<Class>();

    public override string ToString()
    {
        return $"Teacher: {Id}, {FirstName} {LastName}, " +
               $"Classes: {(Classes != null ? Classes.Count : 0)}";
    }
}