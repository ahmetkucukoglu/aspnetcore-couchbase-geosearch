namespace Couchbase.GeoSearchApp
{
    using System;

    public class GetUpcomingEventsResponse
    {
        public string Subject { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Address { get; set; }
    }
}
