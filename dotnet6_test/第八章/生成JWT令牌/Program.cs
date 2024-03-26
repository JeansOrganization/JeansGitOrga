using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var claims = new List<Claim>();
claims.Add(new Claim(ClaimTypes.MobilePhone, "15625866068"));
claims.Add(new Claim(ClaimTypes.Name, "Jean"));
claims.Add(new Claim(ClaimTypes.StreetAddress, "陆丰甲子"));
claims.Add(new Claim(ClaimTypes.Gender, "1"));

DateTime notBefore = DateTime.Now;
DateTime expires = notBefore.AddDays(2);
string key = "今天天气晴朗，晴转多云，真是个不错的好日子";
byte[] secKey = Encoding.UTF8.GetBytes(key);
SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secKey);
SigningCredentials signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);
JwtSecurityToken token = new JwtSecurityToken("poster","Getter",claims,notBefore,expires,signingCredentials);
//JwtSecurityToken token = new JwtSecurityToken(issuer:"poster",audience:"Getter",claims:claims,
//    notBefore:notBefore,expires:expires,signingCredentials: signingCredentials);
string jwt = new JwtSecurityTokenHandler().WriteToken(token);
Console.WriteLine(jwt);
