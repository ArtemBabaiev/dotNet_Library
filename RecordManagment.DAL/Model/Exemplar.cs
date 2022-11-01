using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Model
{
    public class Exemplar :BaseEntity
    {
        public long LiteratureId { get; set; }
        public Boolean IsLend { get; set; }
    }
}
