using Diagramma_Ganta.Context;
using Diagramma_Ganta.Dto;
using Diagramma_Ganta.Logic.IServices;
using Diagramma_Ganta.Logic.PasswordHashing;
using Diagramma_Ganta.Model;
using Microsoft.EntityFrameworkCore;

namespace Diagramma_Ganta.Logic.Services;

public class UserService : IUserService
{
    private readonly IPasswordHash _passwordHash;
    private readonly AppDbContext _db;

    public UserService(IPasswordHash passwordHash,AppDbContext db)
    {
        _passwordHash = passwordHash;
        _db = db;
    }
    
    
    public async Task<Guid> Register(RegistRequest user)
    {
        var newUser = new User
        {
            PasswordHash = _passwordHash.Generate(user.password),
            Email = user.email,
        };
        await _db.Users.AddAsync(newUser);
        await _db.SaveChangesAsync();
        return newUser.Id;
    }
    
    public async Task<string?> Login(LoginDto request)
    {
        if (request == null || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.password))
            return null;
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.email);
        
        if (user == null || !_passwordHash.CheckPassword(request.password, user.PasswordHash))
            return null;
        
            var session = new Session
            {
                Token = Guid.NewGuid(),
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            await _db.Sessions.AddAsync(session);
            await _db.SaveChangesAsync();

            return (session.Token.ToString());

    }
    
    public async Task<bool?> Logout(string stringSessionToken)
    {
        if (string.IsNullOrEmpty(stringSessionToken)) 
            return null;
        
        var sessionToken = Guid.Parse(stringSessionToken);
        var session = await _db.Sessions.FirstOrDefaultAsync(s=>s.Token == sessionToken && s.ExpiresAt > DateTime.UtcNow);
        
        if (session == null)
            return null;
        
        _db.Sessions.Remove(session);
        await _db.SaveChangesAsync();
    
        return true;
    }
    
    public async Task<UserDto?> GetUser(string sessionToken)
    {
        var userDto = await _db.Sessions
            .Where(s => s.Token == Guid.Parse(sessionToken) && s.ExpiresAt > DateTime.UtcNow).Join(_db.Users,
                session => session.UserId,   
                user => user.Id,              
                (session, user) => new UserDto
                {
                    id = user.Id,
                    email = user.Email,
                })
            .FirstOrDefaultAsync();

        return userDto;
    }
    
    public async Task<bool> IsEmailUnique(string email)
    {
        return !await _db.Users.Where(u=> u.Email == email).AnyAsync();
    }
}