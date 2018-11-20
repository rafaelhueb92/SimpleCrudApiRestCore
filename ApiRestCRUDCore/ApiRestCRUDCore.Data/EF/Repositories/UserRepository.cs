using ApiRestCRUDCore.Domain.Contracts;
using ApiRestCRUDCore.Domain.Entities;
using Components.Repositories;
using System.Linq;

namespace ApiRestCRUDCore.Data.EF.Repositories
{

    public class UserRepository : Repository<eUser>, IUserRepository
    {

        public readonly ApiRestCRUDCoreDataContext _ctx;

        public UserRepository(ApiRestCRUDCoreDataContext ctx) : base(ctx) { _ctx = ctx; }

        public eUser Log(string Email, string Password)
        {

            var User =  _ctx.User.FirstOrDefault(l => l.Email.Contains(Email));

            if (User == null) return null;

            else if (Password != User.Password) return null;

            eUser Funcionario = User;

            return Funcionario;

        }

    }

}
