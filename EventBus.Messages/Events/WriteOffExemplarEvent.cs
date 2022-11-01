using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    public class WriteOffExemplarEvent : IntegrationBaseEvent
    {
        public long LiteratureId { get; set; }
        public string Name { get; set; }
        public string Isbn { get; set; }
        public int PublishingYear { get; set; }
        public int Quantity { get; set; }
        public string AuthorName { get; set; }
        public string AuthorDescription { get; set; }
        public string PublisherName { get; set; }
        public string PublisherDescription { get; set; }
        public long EmployeeId { get; set; }
    }
}
