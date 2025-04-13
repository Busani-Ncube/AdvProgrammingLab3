public class Teacher
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Navigation Property for one-to-one
    // A teacher has one class (for a proper one-to-one relationship)
    public Class Class { get; set; }

    public override string ToString()
    {
        return $"Teacher: {Id}, {FirstName} {LastName}";
    }
}
