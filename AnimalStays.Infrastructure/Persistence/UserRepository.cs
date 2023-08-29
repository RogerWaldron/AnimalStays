using AnimalStays.Application.Common.Interfaces.Persistence;
using AnimalStays.Domain.Entities;

namespace AnimalStays.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
  private static List<User> _users = new ();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
      return _users.SingleOrDefault(u => u.Email == email);
    }
}