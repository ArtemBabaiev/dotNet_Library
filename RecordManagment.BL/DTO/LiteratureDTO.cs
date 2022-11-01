using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.DTO
{
    public class LiteratureDTO
    {
        public long Id { get; set; }
        public String? Name { get; set; }
        public String? Isbn { get; set; }
        public int LendTimeInDays { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
