using AtomicSensors.Models;
using AtomicSensors.Services;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Text;

namespace AtomicSensors.Controllers
{
    [ApiController]
    [Route("api")]
    public class SensorDataController: ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public SensorDataController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet("data")]
        public async Task<List<SensorData>> Get(string? sort_mode, string? sort_by, int? id_filter, string? type_filter,
            DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            return await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_from, date_to);
        }

        [HttpGet("data/json")]
        public async Task<List<SensorData>> GetDownloadJson(string? sort_mode, string? sort_by, int? id_filter, string? type_filter, 
            DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            return await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_from, date_to);
        }

        [HttpGet("data/csv")]
        public async Task<string> GetDownloadCsv(string? sort_mode, string? sort_by, int? id_filter, string? type_filter,
            DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            var list = await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_from, date_to);
            return CsvSerializer.SerializeToCsv(list);
        }

    }
}
