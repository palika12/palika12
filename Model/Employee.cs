using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FullStack.API.Model
{
    public class Employee
    {
        [BindNever]
        public Guid id { get; set; }
        public string? name { get; set; }
        public string ?email { get; set; }
        public long phone { get; set; }
       // public int salary { get; set; }
       // public string ?department { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")] 

        [JsonIgnore]
        public Department? Department { get; set; }
        [JsonIgnore]
        public Salary? Salary { get; set; }
    }
}
