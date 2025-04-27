using System;

namespace Shared.Events
{
    public class ReportRequestCreatedEvent
    {
        public Guid ReportId { get; set; }
        public string Location { get; set; }
        public DateTime RequestedAt { get; set; }
    }
}