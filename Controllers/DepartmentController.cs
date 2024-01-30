using FullStack.API.Data;
using FullStack.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FullStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly EmployeeDbcontext _employeeDbContext;

        public DepartmentController(EmployeeDbcontext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _employeeDbContext.Departments.ToListAsync();
            return Ok(departments);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] Department departmentRequest)
        {
            await _employeeDbContext.Departments.AddAsync(departmentRequest);
            await _employeeDbContext.SaveChangesAsync();

           
            var departmentWithEmployees = await _employeeDbContext.Departments
                .Where(d => d.DepartmentId == departmentRequest.DepartmentId)
                .Include(d => d.Employees) 
                .Select(d => new
                {
                    d.DepartmentId,
                    d.Name,
                    d.Discription,
                    Employees = d.Employees.Select(e => new
                    {
                        e.name,
                        e.email,
                        e.phone,
                        // e.department
                    })
                })
                .FirstOrDefaultAsync();

            if (departmentWithEmployees == null)
            {
                return NotFound();
            }

            return Ok(departmentWithEmployees);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetDepartment([FromRoute] int id)
        {
            var departmentWithEmployees = await _employeeDbContext.Departments
                .Where(d => d.DepartmentId == id)
                .Include(d => d.Employees)
                .Select(d => new
                {
                    d.DepartmentId,
                    d.Name,
                    d.Discription,
                    Employees = d.Employees.Select(e => new
                    {
                        e.name,
                        e.email,
                        e.phone,
                        // e.department
                    })
                })
                .FirstOrDefaultAsync();

            if (departmentWithEmployees == null)
            {
                return NotFound();
            }

            return Ok(departmentWithEmployees);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] Guid id, Department updateDepartmentRequest)
        {
            var department = await _employeeDbContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            department.Name = updateDepartmentRequest.Name;
            department.Discription = updateDepartmentRequest.Discription;

            await _employeeDbContext.SaveChangesAsync();

            return Ok(department);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] Guid id)
        {
            var department = await _employeeDbContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _employeeDbContext.Departments.Remove(department);
            await _employeeDbContext.SaveChangesAsync();

            return Ok(department);
        }
        [HttpGet]
        [Route("{departmentId:int}/employees")]
        public async Task<IActionResult> GetEmployeesByDepartment([FromRoute] int departmentId)
        {
            var departmentWithEmployees = await _employeeDbContext.Departments
                .Where(d => d.DepartmentId == departmentId)
                .Include(d => d.Employees)
                .Select(d => new
                {
                    d.DepartmentId, // primary key in the Department
                    d.Name,
                    d.Discription,
                    Employees = d.Employees.Select(e => new
                    {
                        e.name,
                        e.email,
                        e.phone,
                        // e.department
                    })
                })
                .FirstOrDefaultAsync();

            if (departmentWithEmployees == null)
            {
                return NotFound();
            }

            return Ok(departmentWithEmployees);
        }


    }
}