using RecordManagment.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.DTO
{
    public class ExemplarDTO
    {
        public long Id { get; set; }
        public LiteratureDTO? Literature { get; set; }
        public Boolean IsLend { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
