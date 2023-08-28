namespace AnimalStays.Infrastructure.Authentication;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnimalStays.Application.Common.Interfaces.Authentication;
using AnimalStays.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


public class JwtTokenGenerator : IJwtTokenGenerator
{
  private readonly IDateTimeProvider _dateTimeProvider;
  private readonly JwtSettings _jwtOptions;

    public JwtTokenGenerator(
      IDateTimeProvider dateTimeProvider, 
      IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(Guid userId, string firstName, string lastName) 
    {
      if (string.IsNullOrWhiteSpace(firstName) || 
          string.IsNullOrWhiteSpace(lastName))
      {
          throw new ArgumentException("Error: Params cannot be null or whitespace.");
      }

      var claims = new[] 
      {
        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new Claim(JwtRegisteredClaimNames.GivenName, firstName),
        new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };

      var signingCredentials =  new SigningCredentials(
        new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
        SecurityAlgorithms.HmacSha256);
      // 40 char key otherwise error about not enough bytes for hash algo

      var securityToken = new JwtSecurityToken(
        issuer: _jwtOptions.Issuer,
        expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
        claims: claims,
        signingCredentials: signingCredentials);
      
      return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}