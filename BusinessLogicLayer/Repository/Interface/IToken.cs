using Entities;

namespace Repository
{
    public interface IToken
    {
        Token GenerateToken(int userId);
        bool IsTokenValid(Token token);
    }
}
