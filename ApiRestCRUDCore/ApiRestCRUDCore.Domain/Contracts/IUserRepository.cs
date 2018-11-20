using ApiRestCRUDCore.Domain.Entities;
using Components.Contracts;

namespace ApiRestCRUDCore.Domain.Contracts
{
    public interface IUserRepository :  IRepository<eUser>
    {
         eUser Log(string Email, string Password);
    }
}
