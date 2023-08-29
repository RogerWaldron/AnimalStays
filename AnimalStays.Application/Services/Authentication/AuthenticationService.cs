using AnimalStays.Application.Common.Interfaces.Authentication;
using AnimalStays.Application.Common.Interfaces.Persistence;
using AnimalStays.Domain.Entities;

namespace AnimalStays.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
   private readonly IJwtTokenGenerator _jwtTokenGenerator;
   private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
      // validate the user doesn't exist
      if (_userRepository.GetUserByEmail(email) is not null)
      {
        throw new Exception("a user already exists with that email address");
      }

      // create user with unique Id & persist
      var user = new User {
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        Password = password
      };

      // create JWT token
      _userRepository.Add(user);

      var token = _jwtTokenGenerator.GenerateToken(user);

      return new AuthenticationResult(
        user,
        token
      );
    }

    public AuthenticationResult Login(string email, string password)
    {
      // validate user exists
      if (_userRepository.GetUserByEmail(email) is not User user)
      {
        throw new Exception("Unable to login user with email");
      }
      // validate password
      if (user.Password != password)
      {
        throw new Exception("Invalid password");
      }
      // create JWT 
      var token = _jwtTokenGenerator.GenerateToken(user); 

      return new AuthenticationResult(
        user, 
        token);
    }
}