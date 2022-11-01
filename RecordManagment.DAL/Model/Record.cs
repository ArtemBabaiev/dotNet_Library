using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Model
{
    public class Record: BaseEntity
    {
        public long ExemplarId { get; set; }
        public long ReaderId { get; set; }
        public long LendByEmployeeId { get; set; }
        public long AcceptedByEmployeeId { get; set; }
        public DateTime? LendAt{ get; set; }
        public DateTime? AcceptedAt{ get; set; }
    }
}
