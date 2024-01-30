using FullStack.API.Data;
using FullStack.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly EmployeeDbcontext _employeeDbContext;

        public SalaryController(EmployeeDbcontext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSalaries()
        {
            var salariesWithEmployees = await _employeeDbContext.Salaries
                .Include(s => s.Employee)
                .Select(s => new
                {
                    s.Id,
                    s.Amount,
                    s.EmployeeId,
                    Employee = new
                    {
                        s.Employee.name,
                        s.Employee.email,
                        s.Employee.phone,
                     //   s.Employee.salary,
                      //  s.Employee.department
                    }
                })
                .ToListAsync();

            return Ok(salariesWithEmployees);
        }

        [HttpPost]
        public async Task<IActionResult> AddSalary([FromBody] Salary salaryRequest)
        {
            salaryRequest.Id = Guid.NewGuid();
            await _employeeDbContext.Salaries.AddAsync(salaryRequest);
            await _employeeDbContext.SaveChangesAsync();
            return Ok(salaryRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetSalary([FromRoute] Guid id)
        {
            var salaryWithEmployee = await _employeeDbContext.Salaries
                .Where(s => s.Id == id)
                .Include(s => s.Employee) 
                .Select(s => new
                {
                    s.Id,
                    s.Amount,
                    s.EmployeeId,
                    Employee = new
                    {
                        s.Employee.name,
                        s.Employee.email,
                        s.Employee.phone,
                       // s.Employee.salary,
                      //  s.Employee.department
                    }
                })
                .FirstOrDefaultAsync();

            if (salaryWithEmployee == null)
            {
                return NotFound();
            }

            return Ok(salaryWithEmployee);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateSalary([FromRoute] Guid id, Salary updateSalaryRequest)
        {
            var salary = await _employeeDbContext.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

           
            salary.Amount = updateSalaryRequest.Amount;
            salary.EmployeeId = updateSalaryRequest.EmployeeId;

            await _employeeDbContext.SaveChangesAsync();

            return Ok(salary);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteSalary([FromRoute] Guid id)
        {
            var salary = await _employeeDbContext.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            _employeeDbContext.Salaries.Remove(salary);
            await _employeeDbContext.SaveChangesAsync();

            return Ok(salary);
        }
    }
}