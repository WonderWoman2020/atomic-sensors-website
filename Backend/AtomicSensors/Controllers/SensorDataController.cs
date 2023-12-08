using AtomicSensors.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtomicSensors.Controllers
{
    [Controller]
    [Route("api")]
    public class SensorDataController: Controller
    {
        private readonly MongoDBService _mongoDBService;

        public SensorDataController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet("data")]
        public async Task<List<SensorData>> Get(string? sort_mode, string? sort_by, int? id_filter, string? type_filter, string? date_filter)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}");
            return await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_filter);
        }
    }
}
