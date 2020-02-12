namespace Couchbase.GeoSearchApp
{
    using System;

    public class EventDocument
    {
        public string Subject { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public EventLocationDocument Location { get; set; }
    }

    public class EventLocationDocument
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
