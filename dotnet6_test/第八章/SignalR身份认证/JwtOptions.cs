namespace SignalR身份认证
{
    public class JwtOptions
    {
        public string? SigningKey { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
