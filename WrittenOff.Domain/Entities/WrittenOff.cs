using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.ValueObject;

namespace WrittenOffManagement.Domain.Entities
{
    public class WrittenOff
    {
        public long Id { get; set; }
        public string Name { get; set; } 
        public string Isbn { get; set; }
        public int PublishingYear { get; set; }
        public int Quantity { get; set; }
        public Author Author { get; set; }
        public Publisher Publisher{ get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt{ get; set; }
        public long EmployeeId { get; set; }

        public virtual Employee Employee { get; set; } = null!;


    }
}
