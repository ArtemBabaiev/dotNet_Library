using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.DTO
{
    public class RecordDTO
    {
        public long Id { get; set; }
        public ExemplarDTO? Exemplar{ get; set; }
        public ReaderDTO? Reader { get; set; }
        public EmployeeDTO? LendByEmployee { get; set; }
        public EmployeeDTO? AcceptedByEmployee { get; set; }
        public DateTime? LendAt { get; set; }
        public DateTime? AcceptedAt { get; set; }
    }
}
