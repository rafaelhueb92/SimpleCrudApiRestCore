using ApiRestCRUDCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiRestCRUDCore.Domain.Contracts
{

    public interface IEmployeeRepository
    {

        Task<List<eEmployee>> GetAll();

        Task<eEmployee> GetByID(int id);

        Task<List<eEmployee>> GetByDateOfBirth(DateTime dateOfBirth);

        Task<eEmployee> Add(eEmployee employee);

        Task<eEmployee> Update(eEmployee employee);

        void Delete(int id);

    }

}
