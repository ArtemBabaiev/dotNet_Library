using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.DTO
{
    public class ReaderDTO
    {
        public long Id { get; set; }
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public String? Address { get; set; }
        public String? PhoneNumber { get; set; }
        public String? ReaderTicketNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
