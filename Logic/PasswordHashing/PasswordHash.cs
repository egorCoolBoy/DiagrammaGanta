namespace Diagramma_Ganta.Logic.PasswordHashing;

public class PasswordHash : IPasswordHash
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool CheckPassword(string password,string passwordHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
    
}