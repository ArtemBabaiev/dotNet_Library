using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittenOffManagement.Domain.ValueObject;

namespace WrittenOffManagement.Application.DTO.Response
{
    public class EmployeeResponse
    {
        public long Id { get; set; }
        public EmployeeName Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
