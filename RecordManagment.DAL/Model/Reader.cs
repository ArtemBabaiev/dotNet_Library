using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Model
{
    public class Reader : BaseEntity
    {
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public String? Address { get; set; }
        public String? PhoneNumber { get; set; }
        public String? ReaderTicketNumber { get; set; }
    }
}
