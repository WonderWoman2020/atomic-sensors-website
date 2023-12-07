using AtomicSensors.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

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

    public async Task<List<SensorData>> GetAsync()
    {
        return await _sensorDataCollection.Find(new BsonDocument()).ToListAsync();
    }
    public async Task CreateAsync(SensorData sensorData) 
    {
        await _sensorDataCollection.InsertOneAsync(sensorData);
        return;
    }
}
