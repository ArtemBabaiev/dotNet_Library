namespace Aggregator.DTO
{
    public class LiteratureAggDTO
    {
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
    }
}
