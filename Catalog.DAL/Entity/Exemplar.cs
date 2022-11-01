using System;
using System.Collections.Generic;

namespace Catalog.DAL.Entity
{
    public class Exemplar
    {
        public long Id { get; set; }
        public long LiteratureId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsLend { get; set; }

        public virtual Literature Literature { get; set; } = null!;
    }
}
