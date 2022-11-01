using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Model
{
    public class Employee:BaseEntity
    {
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
    }
}
