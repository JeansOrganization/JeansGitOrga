namespace ASPNETCore_JWT
{
    public class JwtSetting
    {
        public string? SigningKey { get; set; }
        public int ExpireSeconds { get; set; }
    }
}
