using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FullStack.API.Model
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        public string ?Name { get; set; }
        public string ?Discription { get; set; }
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}

