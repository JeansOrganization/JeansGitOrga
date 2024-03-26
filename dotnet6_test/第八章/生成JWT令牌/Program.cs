using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

var claims = new List<Claim>();
claims.Add(new Claim(ClaimTypes.NameIdentifier, "2"));
claims.Add(new Claim(ClaimTypes.Name, "Jean"));
claims.Add(new Claim(ClaimTypes.MobilePhone, "15625866068"));

DateTime notBefore = DateTime.Now;
DateTime expires = notBefore.AddDays(2);
string key = "今天是2024年3月26号21点45分";
byte[] secKey = Encoding.UTF8.GetBytes(key);
SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secKey);
SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

JwtSecurityToken token = new JwtSecurityToken("Poster","Getter",claims,notBefore,expires,signingCredentials);
//JwtSecurityToken token = new JwtSecurityToken(issuer:"Poster",audience:"Getter", claims:claims, 
//    notBefore:notBefore, expires:expires, signingCredentials:signingCredentials);
JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
string jwt = handler.WriteToken(token);
Console.WriteLine(jwt);