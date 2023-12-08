using AtomicSensors.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

using System.Linq;

namespace AtomicSensors.Services;

public class MongoDBService
{
    private readonly IMongoCollection<SensorData> _sensorDataCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _sensorDataCollection = database.GetCollection<SensorData>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<SensorData>> GetAsync(string sort_mode, string sort_by, int id_filter, string type_filter, string date_filter)
    {
        var filterBuilder = Builders<SensorData>.Filter;
        var filter = filterBuilder.Eq("SensorId", id_filter) & filterBuilder.Eq("SensorType", type_filter);
        var sortBuilder = Builders<SensorData>.Sort;
        var sort = sort_mode == "asc" ? sortBuilder.Ascending(sort_by) : sortBuilder.Descending(sort_by);
        //return await _sensorDataCollection.Find(new BsonDocument()).ToListAsync();

        //return await _sensorDataCollection.AsQueryable<SensorData>().Where(sensor => sensor.SensorId == id_filter);

        return await _sensorDataCollection.Find<SensorData>(filter).Sort(sort).ToListAsync();
    }
    public async Task CreateAsync(SensorData sensorData) 
    {
        await _sensorDataCollection.InsertOneAsync(sensorData);
        return;
    }
}
