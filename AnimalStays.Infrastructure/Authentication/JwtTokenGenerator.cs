namespace AnimalStays.Infrastructure.Authentication;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnimalStays.Application.Common.Interfaces.Authentication;
using AnimalStays.Application.Common.Interfaces.Services;
using AnimalStays.Domain.Entities;
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

    public string GenerateToken(User user) 
    {
      var signingCredentials =  new SigningCredentials(
        new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(_jwtOptions.Secret)),
        SecurityAlgorithms.HmacSha256);
      // 40 char key otherwise error about not enough bytes for hash algo

      var claims = new[] 
      {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
        new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
      };

      var securityToken = new JwtSecurityToken(
        issuer: _jwtOptions.Issuer,
        audience: _jwtOptions.Audience,
        expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes),
        claims: claims,
        signingCredentials: signingCredentials);
      
      return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}