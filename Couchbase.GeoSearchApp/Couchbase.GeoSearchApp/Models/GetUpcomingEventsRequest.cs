namespace Couchbase.GeoSearchApp
{
    public class GetUpcomingEventsRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Radius { get; set; }
    }
}
