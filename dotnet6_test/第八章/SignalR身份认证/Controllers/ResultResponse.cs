namespace SignalR身份认证.Controllers
{
    public class ResultResponse<T>
    {
        public string? Code { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
        public T? Obj { get; set; }
        public ResultResponse(string Code,string Error)
        {
            this.Code = Code;
            this.Error = Error;
        }

        public ResultResponse(string Code, string Error,string Token,T? Obj):this(Code,Error)
        {
            this.Token = Token;
            this.Obj = Obj;
        }
    }
}
