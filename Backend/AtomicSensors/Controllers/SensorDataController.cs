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
            // Do testowania
            /*var data1 = new SensorData(1, "Pressure", 112.5);
            var data2 = new SensorData(2, "Temperature", 135.7);
            var list = new List<SensorData>();
            list.Add(data1);
            list.Add(data2);
            return list;*/

        }

        /*[HttpPost]
        public async Task<IActionResult> Post([FromBody] Playlist playlist) { }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddToPlaylist(string id, [FromBody] string movieId) { }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) { }*/
    }
}
