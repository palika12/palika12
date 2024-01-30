using FullStack.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FullStack.API.Data
{
    public class EmployeeDbcontext:DbContext
    {
        public EmployeeDbcontext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .IsRequired(false); // Adjust this based on your requirements
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Salary> Salaries { get; set;}
        public DbSet<Department> Departments { get; set; }
        
    }
}
