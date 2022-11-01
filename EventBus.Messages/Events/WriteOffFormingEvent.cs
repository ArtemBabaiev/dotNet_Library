using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class WriteOffFormingEvent : IntegrationBaseEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Isbn { get; set; }
        public int PublishingYear { get; set; }
        public int Quantity { get; set; }
        public long AuthorId { get; set; }
        public long  PublisherId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long EmployeeId { get; set; }
    }
}
