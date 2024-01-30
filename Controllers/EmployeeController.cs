using FullStack.API.Data;
using FullStack.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private EmployeeDbcontext _employeeDbContext;
        public EmployeeController(EmployeeDbcontext employeeContext)
        {
            this._employeeDbContext = employeeContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeDbContext.Employees.ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            //  employeeRequest.id = Guid.NewGuid();
            try
                {
                await _employeeDbContext.Employees.AddAsync(employeeRequest);
                await _employeeDbContext.SaveChangesAsync();
                return Ok(employeeRequest);
            }
            catch (Exception ex)
            
            {
                Debugger.Break();
               return BadRequest(ex.Message);
            }
         
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _employeeDbContext.Employees
                .FirstOrDefaultAsync(x => x.id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee updateEmployeeRequest)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.name = updateEmployeeRequest.name;
            employee.email = updateEmployeeRequest.email;
            employee.phone = updateEmployeeRequest.phone;
           // employee.salary = updateEmployeeRequest.salary;
          //  employee.department = updateEmployeeRequest.department;

            await _employeeDbContext.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _employeeDbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            _employeeDbContext.Employees.Remove(employee);
            await _employeeDbContext.SaveChangesAsync(true);
            return Ok(employee);
        }
        [HttpGet]
        [Route("details/{employeeId:Guid}")]
        public async Task<IActionResult> GetEmployeeDetails([FromRoute] Guid employeeId)
        {
            var employeeDetails = await _employeeDbContext.Employees
                .Where(e => e.id == employeeId)
                .Include(e => e.Department)
                .Include(e => e.Salary)
                .Select(e => new
                {
                    e.id,
                    e.name,
                    e.email,
                    e.phone,
                    Department = new
                    {
                        e.Department.Name,
                        e.Department.Discription
                    },
                    Salary = new
                    {
                        e.Salary.Amount
                    }
                    // Add more properties as needed
                })
                .FirstOrDefaultAsync();

            if (employeeDetails == null)
            {
                return NotFound();
            }

            return Ok(employeeDetails);
        }

    }
}
        
    
