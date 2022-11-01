using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.DTO.Request
{
    public class ExemplarRequest
    {
        public long Id { get; set; }
        public long LiteratureId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsLend { get; set; }
    }
}
