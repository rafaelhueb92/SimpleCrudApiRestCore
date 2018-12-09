using ApiRestCRUDCore.Domain.Contracts;
using ApiRestCRUDCore.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRestCRUDCore.Data.Dapper.Repositories
{

    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly IConfiguration _config;

        public EmployeeRepository(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("ApiConnection"));
            }
        }

        public async Task<List<eEmployee>> GetAll()
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT * FROM Employee";
                conn.Open();
                var result = await conn.QueryAsync<eEmployee>(sQuery,new { });
                return result.ToList();
            }
        }

        public async Task<List<eEmployee>> GetByDateOfBirth(DateTime dateOfBirth)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT ID, FirstName, LastName, DateOfBirth FROM Employee WHERE DateOfBirth = @DateOfBirth";
                conn.Open();
                var result = await conn.QueryAsync<eEmployee>(sQuery, new { DateOfBirth = dateOfBirth });
                return result.ToList();
            }
        }

        public async Task<eEmployee> GetByID(int id)
        {

            using (IDbConnection conn = Connection)
            {
                string sQuery = "SELECT Id, FirstName, LastName, DateOfBirth FROM Employee WHERE Id = @Id";
                conn.Open();
                var result = await conn.QueryAsync<eEmployee>(sQuery, new { Id = id });
                return result.FirstOrDefault();
            }

        }

        public async Task<eEmployee> Add(eEmployee employee)
        {

            using (IDbConnection conn = Connection)
            {
                string sQuery = @"INSERT INTO Employee VALUES (@Id,@FirstName,@LastName,@DateOfBirth,@Login,@Password);";
                conn.Open();
                var result = await conn.QueryAsync<eEmployee>(sQuery, 
                              new {
                                    Id = employee.Id,
                                    FirstName = employee.FirstName,
                                    LastName = employee.LastName,
                                    DateOfBirth = employee.DateOfBirth,
                                    Login = employee.Login,
                                    Password = employee.Password
                                   });
                return employee;
            }

        }

        public async Task<eEmployee> Update(eEmployee employee)
        {
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"UPDATE Employee
                                     SET FirstName = @FirstName,
	                                     LastName = @LastName,
	                                     DateOfBirth = @DateOfBirth,
                                      	 Login = @Login,
	                                     Password = @Password
                                         WHERE Id = @Id;";
                conn.Open();
                var result = await conn.QueryAsync<eEmployee>(sQuery,
                              new
                              {
                                  Id = employee.Id,
                                  FirstName = employee.FirstName,
                                  LastName = employee.LastName,
                                  DateOfBirth = employee.DateOfBirth,
                                  Login = employee.Login,
                                  Password = employee.Password
                              });
                return employee;
            }
        }

        public void Delete(int id)
        {
            
            using (IDbConnection conn = Connection)
            {
                string sQuery = @"DELETE FROM Employee
                                        WHERE Id = @Id;";
                conn.Open();
                var result = conn.QueryAsync<eEmployee>(sQuery,
                              new
                              {
                                  Id = id
                              });

            }
        }
    }

}
