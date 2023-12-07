using AtomicSensors.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtomicSensors.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class SensorDataController: Controller
    {
        private readonly MongoDBService _mongoDBService;

        public SensorDataController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<SensorData>> Get()
        {
            return await _mongoDBService.GetAsync();
        }
    }
}
