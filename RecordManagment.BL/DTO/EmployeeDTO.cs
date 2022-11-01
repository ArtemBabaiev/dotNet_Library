using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.DTO
{
    public class EmployeeDTO
    {
        public long Id { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
