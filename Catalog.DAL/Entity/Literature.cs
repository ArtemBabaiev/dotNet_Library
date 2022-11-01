using System;
using System.Collections.Generic;

namespace Catalog.DAL.Entity
{
    public partial class Literature
    {
        public Literature()
        {
            Exemplars = new HashSet<Exemplar>();
            Writings = new HashSet<Writing>();
        }

        public long Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Description { get; set; }
        public string? Isbn { get; set; }
        public string? Name { get; set; }
        public int? NumberOfPages { get; set; }
        public int? PublishingYear { get; set; }
        public long PublisherId { get; set; }
        public long AuthorId { get; set; }
        public long GenreId { get; set; }
        public long TypeId { get; set; }
        public bool? IsLendable { get; set; }
        public int? LendPeriodInDays { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
        public virtual Publisher Publisher { get; set; } = null!;
        public virtual Type Type { get; set; } = null!;
        public virtual ICollection<Exemplar> Exemplars { get; set; }

        public virtual ICollection<Writing> Writings { get; set; }
    }
}
