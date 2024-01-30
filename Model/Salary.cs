using System.Text.Json.Serialization;

namespace FullStack.API.Model
{
    public class Salary
    {

        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Guid EmployeeId { get; set; } //fk
        [JsonIgnore]
       public Employee? Employee { get; set; }
    }
}

