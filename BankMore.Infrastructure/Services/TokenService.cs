using BankMore.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankMore.Infrastructure.Services
{
    public class TokenService
    {
        public string GerarToken(ContaCorrente conta)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // Em produção, essa chave deve vir do appsettings.json
            var chave = Encoding.ASCII.GetBytes("SuaChaveSecretaMuitoLongaEProtegida123!");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, conta.Nome),            
                    // AJUSTE AQUI: Mudamos de .Numero para .NumeroDaConta
                    new Claim("NumeroConta", conta.NumeroDaConta.ToString()),
                    new Claim("IdUsuario", conta.Id.ToString()),
                    new Claim("Cpf", conta.Cpf)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chave),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}