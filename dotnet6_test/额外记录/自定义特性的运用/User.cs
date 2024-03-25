namespace 自定义特性的运用
{
    public class User
    {
        [Length(3,5)]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
