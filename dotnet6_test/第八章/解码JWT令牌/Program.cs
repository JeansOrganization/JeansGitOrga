
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


/* 使用JwtSecurityTokenHandler解码,具有验证效果 */
string? jwt = Console.ReadLine();
string key = "今天是2024年3月26号21点45分";
SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
TokenValidationParameters validParams = new TokenValidationParameters();
validParams.IssuerSigningKey = securityKey;
validParams.ValidIssuers = new string[] { "Poster", "Sender" };
validParams.ValidAudiences = new string[] { "Getter", "Reciever" };
validParams.ValidateIssuer = true;
validParams.ValidateAudience = true;
JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
ClaimsPrincipal principal;
try
{
    principal = handler.ValidateToken(jwt, validParams, out SecurityToken validatedToken);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return;
}
foreach (var claim in principal.Claims)
{
    Console.WriteLine($"{claim.ValueType}:{claim.Value}");
}

/* 普通解码 */
//string? jwt = Console.ReadLine();
//string[] segments = jwt!.Split('.');
//string head = segments[0];
//string payload = segments[1];
//string sign = segments[2];

//string headJson = JwtDecode(head);
//string payloadJson = JwtDecode(payload);
//string signJson = JwtDecode(sign);
//Console.WriteLine(headJson);
//Console.WriteLine(payloadJson);

//string JwtDecode(string s)
//{
//    s = s.Replace('-', '+').Replace('_', '/');
//    switch (s.Length % 4)
//    {
//        case 2:
//            s += "==";
//            break;
//        case 3:
//            s += "=";
//            break;
//    }
//    var bytes = Convert.FromBase64String(s);
//    return Encoding.UTF8.GetString(bytes);
//}

