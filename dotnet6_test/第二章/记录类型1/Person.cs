public record Person(string firstName, string lastName)
{
    public int Age { get; set; }

    public string GetDisplayName() => $"{this.firstName}-{lastName}";
}