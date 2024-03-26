
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

string? jwt = Console.ReadLine();
string key = "今天天气晴朗，晴转多云，真是个不错的好日子";
byte[] secKey = Encoding.UTF8.GetBytes(key);
SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secKey);
TokenValidationParameters validationParameters = new TokenValidationParameters();
validationParameters.IssuerSigningKey = securityKey;
validationParameters.ValidateIssuer = false;
validationParameters.ValidateAudience = false;

JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
ClaimsPrincipal claimsPrincipal = 
    jwtSecurityTokenHandler.ValidateToken(jwt, validationParameters,out SecurityToken validatedToken);

foreach (var claim in claimsPrincipal.Claims)
{
    Console.WriteLine($"{claim.Type}:{claim.Value}");
}
