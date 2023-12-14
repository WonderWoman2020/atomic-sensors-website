using AtomicSensors.Models;
using AtomicSensors.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
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

        /// <summary>
        /// Pobiera dane w formacie Json. Pozwala na filtrowanie i sortowanie.
        /// </summary>
        /// <param name="sort_mode" example="asc">Kierunek sortowania. Akceptowane wartości: asc, desc.</param>
        /// <param name="sort_by" example="Data">Nazwa kolumny danych, według której chcemy sortować. Akceptowane wartości: SensorId, SensorType, Data, Date.</param>
        /// <param name="id_filter" example="1">Filtr na id sensora. Używane wartości: od 0 do 3.</param>
        /// <param name="type_filter" example="Pressure">Filtr na typ sensora. Używane wartości: Temperature, Pressure, Radiation, Seismometer.</param>
        /// <param name="date_from" example="2023-12-08T20:00:00Z">Dolna granica zakresu dat. Parametr może być użyty sam lub razem z parametrem date_to.</param>
        /// <param name="date_to" example="2023-12-10T20:00:00Z">Tak samo jak dla date_from, tylko to górna granica przedziału dat.</param>
        /// <remarks>
        /// Przykładowy request:
        /// 
        ///     GET /api/data?sort_mode=asc&amp;sort_by=SensorId&amp;type_filter=Temperature&amp;date_from=2023-12-08T20:00:00Z
        /// 
        /// </remarks>
        /// <returns>Zwraca listę z obiektami w formacie Json.</returns>
        [HttpGet("data")]
        public async Task<List<SensorData>> Get(string? sort_mode, string? sort_by, int? id_filter, string? type_filter,
            DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            return await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_from, date_to);
        }

        /// <summary>
        /// (Tak samo jak w /api/data) Pobiera dane w formacie Json. Pozwala na filtrowanie i sortowanie.
        /// </summary>
        /// <remarks>
        /// Przykładowy request:
        /// 
        ///     GET /api/data/json?sort_mode=desc&amp;sort_by=Data&amp;type_filter=Radiation&amp;date_from=2023-12-08T08:00:00Z
        /// 
        /// </remarks>
        [HttpGet("data/json")]
        public async Task<List<SensorData>> GetDownloadJson(string? sort_mode, string? sort_by, int? id_filter, string? type_filter, 
            DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            return await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_from, date_to);
        }

        /// <summary>
        /// (Tak samo jak w /api/data, ale zwraca csv) Pobiera dane w formacie csv. Pozwala na filtrowanie i sortowanie.
        /// </summary>
        /// <remarks>
        /// Przykładowy request:
        /// 
        ///     GET /api/data/csv?sort_mode=asc&amp;sort_by=Data&amp;type_filter=Seismometer&amp;date_from=2023-12-08T10:00:00Z
        /// 
        /// </remarks>
        [HttpGet("data/csv")]
        public async Task<string> GetDownloadCsv(string? sort_mode, string? sort_by, int? id_filter, string? type_filter,
            DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            var list = await _mongoDBService.GetAsync(sort_mode, sort_by, id_filter, type_filter, date_from, date_to);
            return CsvSerializer.SerializeToCsv(list);
        }

        [HttpGet("data/stats")]
        public async Task<string> GetStats(string? sort_mode, string? sort_by, int? id_filter, string? type_filter, DateTime? date_from, DateTime? date_to)
        {
            Console.WriteLine($"Sort mode: {sort_mode}, Sort by: {sort_by}, Id filter: {id_filter}, Type filter: {type_filter}, " +
                $"Date from: {date_from}, Date to: {date_to}");
            var list = await _mongoDBService.GetAsync("desc", "Date", null, type_filter, null, null);
            var lastHundredResults = list.Take(100);
            //Console.WriteLine(lastHundredResults.ElementAt(0));
            //Console.WriteLine(lastHundredResults.ToList().Count);
            //Console.WriteLine(lastHundredResults.ToList());
            double mean = lastHundredResults.Average(val => val.Data);
            double last = lastHundredResults.ElementAt(0).Data;
            Console.WriteLine("[{\"mean\": " + mean + ", \"last\": " + last + "}]");
            return "[{\"mean\": "+mean+", \"last\": "+last+"}]";
        }

    }
}
