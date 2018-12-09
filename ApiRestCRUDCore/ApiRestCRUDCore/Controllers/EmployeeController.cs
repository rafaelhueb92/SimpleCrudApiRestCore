using ApiRestCRUDCore.Domain.Contracts;
using ApiRestCRUDCore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRestCRUDCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController: ControllerBase
    {

        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<eEmployee>>> Get()
        {
            return await _employeeRepo.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<eEmployee>> GetByID(int id)
        {
            return await _employeeRepo.GetByID(id);
        }

        [HttpGet]
        [Route("dob/{dateOfBirth}")]
        public async Task<ActionResult<List<eEmployee>>> GetByID(DateTime dateOfBirth)
        {
            return await _employeeRepo.GetByDateOfBirth(dateOfBirth);
        }

        [HttpPost]
        public async Task<ActionResult<eEmployee>> Add([FromBody] eEmployee employee)
        {
            return await _employeeRepo.Add(employee);
        }

        [HttpPut]
        public async Task<ActionResult<eEmployee>> Update([FromBody] eEmployee employee)
        {
            return await _employeeRepo.Update(employee);
        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            _employeeRepo.Delete(id);
            return Ok();
        }

    }

}
