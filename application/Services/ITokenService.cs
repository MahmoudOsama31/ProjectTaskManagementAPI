using ProjectTaskManagement.Models;

namespace ProjectTaskManagement.Services;

public interface ITokenService
{
    string CreateToken(User user);
}
