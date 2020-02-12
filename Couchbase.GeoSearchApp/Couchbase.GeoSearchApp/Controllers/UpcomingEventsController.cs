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

    [Route("api/upcoming-events")]
    [ApiController]
    public class UpcomingEventsController : ControllerBase
    {
        private readonly IBucket _bucket;

        public UpcomingEventsController(IBucketProvider bucketProvider)
        {
            _bucket = bucketProvider.GetBucket("events");
        }

        [HttpGet]
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