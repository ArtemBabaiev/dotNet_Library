using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.ValueObject;

namespace WrittenOffManagement.Domain.Entities
{
    public class Employee
    {
        public long Id { get; set; }
        public EmployeeName Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<WrittenOff> WrittenOffs { get; set; }


        public Employee()
        {
            WrittenOffs = new HashSet<WrittenOff>();
        }

        public Employee(EmployeeName name, DateTime? createdAt, DateTime? updatedAt)
        {
            Name = name;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            WrittenOffs = new HashSet<WrittenOff>();
        }

        public Employee(long id, EmployeeName name, DateTime? createdAt, DateTime? updatedAt)
        {
            Id = id;
            Name = name;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            WrittenOffs = new HashSet<WrittenOff>();
        }
    }
}
