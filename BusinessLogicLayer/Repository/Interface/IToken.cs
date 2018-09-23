namespace Repository
{
    public interface IToken
    {
        string GenerateToken();
        bool IsTokenValid(string accessToken);
    }
}
