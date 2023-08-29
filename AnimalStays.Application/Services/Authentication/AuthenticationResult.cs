using AnimalStays.Domain.Entities;

namespace AnimalStays.Application.Services.Authentication;

public record AuthenticationResult(
  User User,
  string Token
);