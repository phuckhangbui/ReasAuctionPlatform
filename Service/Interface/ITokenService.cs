using BusinessObject.Entity;

namespace Service.Interface
{
    public interface ITokenService
    {
        string CreateToken(Account account);
    }
}
