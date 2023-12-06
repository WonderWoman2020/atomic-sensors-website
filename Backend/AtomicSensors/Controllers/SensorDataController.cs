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

        /*[HttpPost]
        public async Task<IActionResult> Post([FromBody] Playlist playlist) { }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddToPlaylist(string id, [FromBody] string movieId) { }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) { }*/
    }
}
