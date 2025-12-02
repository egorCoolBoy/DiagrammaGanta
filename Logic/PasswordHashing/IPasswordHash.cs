namespace Diagramma_Ganta.Logic.PasswordHashing;

public interface IPasswordHash
{
    public string Generate(string password);
    public bool CheckPassword(string password,string passwordHash);
}