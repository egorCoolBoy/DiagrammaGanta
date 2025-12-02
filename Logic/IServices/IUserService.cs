

using Diagramma_Ganta.Dto;

namespace Diagramma_Ganta.Logic.IServices;

public interface IUserService
{
    public Task<bool> IsEmailUnique(string email);
    public Task<Guid> Register(RegistRequest user);
    public  Task<string?> Login(LoginDto request);
    Task<bool?> Logout(string sessionToken);
    Task<UserDto?> GetUser(string sessionToken); 
}