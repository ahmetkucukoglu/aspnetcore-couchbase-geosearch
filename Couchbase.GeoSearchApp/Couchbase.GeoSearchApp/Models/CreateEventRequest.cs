namespace Couchbase.GeoSearchApp
{
    using System;

    public class CreateEventRequest
    {
        public string Subject { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
