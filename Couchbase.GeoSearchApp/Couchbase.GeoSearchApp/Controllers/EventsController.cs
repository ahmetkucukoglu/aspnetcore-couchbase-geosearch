namespace Couchbase.GeoSearchApp.Controllers
{
    using Couchbase.Core;
    using Couchbase.Extensions.DependencyInjection;
    using Couchbase.Search;
    using Couchbase.Search.Queries.Geo;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IBucket _bucket;

        public EventsController(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("events");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            var document = new Document<EventDocument>
            {
                Id = Guid.NewGuid().ToString(),
                Content = new EventDocument
                {
                    Subject = request.Subject,
                    Description = request.Description,
                    Date = request.Date,
                    Address = request.Address,
                    Location = new EventLocationDocument
                    {
                        Lat = request.Latitude,
                        Lon = request.Longitude
                    }
                }
            };

            await _bucket.InsertAsync(document);

            return Ok();
        }

        [HttpGet]
        [Route("upcoming-events")]
        public async Task<IActionResult> GetUpcomingEvents([FromQuery] GetUpcomingEventsRequest request)
        {
            var query = new GeoDistanceQuery();
            query.Field("location");
            query.Latitude(request.Latitude);
            query.Longitude(request.Longitude);
            query.Distance($"{request.Radius}km");

            var searchParams = new SearchParams()
                .Fields("*")
                .Limit(10)
                .Timeout(TimeSpan.FromMilliseconds(10000));

            var searchQuery = new SearchQuery
            {
                Query = query,
                Index = "eventsgeoindex",
                SearchParams = searchParams
            };

            var searchQueryResults = await _bucket.QueryAsync(searchQuery);

            var result = new List<GetUpcomingEventsResponse>();

            foreach (var hit in searchQueryResults.Hits)
            {
                result.Add(new GetUpcomingEventsResponse
                {
                    Subject = hit.Fields["subject"],
                    Address = hit.Fields["address"],
                    Date = hit.Fields["date"]
                });
            }

            return Ok(result);
        }
    }
}