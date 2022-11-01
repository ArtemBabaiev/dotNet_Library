using System;
using System.Collections.Generic;

namespace Catalog.DAL.Entity
{
    public partial class Genre
    {
        public Genre()
        {
            Literatures = new HashSet<Literature>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Literature> Literatures { get; set; }
    }
}
