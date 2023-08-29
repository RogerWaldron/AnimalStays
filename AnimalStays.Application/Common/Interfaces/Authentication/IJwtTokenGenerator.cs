using AnimalStays.Domain.Entities;

namespace AnimalStays.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
  string GenerateToken(User user);
}