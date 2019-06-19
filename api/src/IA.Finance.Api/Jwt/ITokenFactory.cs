namespace IA.Finance.Api.Jwt
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}