using AnimalStays.Domain.Entities;

namespace AnimalStays.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
  void Add(User user);

  User? GetUserByEmail(string email);
}