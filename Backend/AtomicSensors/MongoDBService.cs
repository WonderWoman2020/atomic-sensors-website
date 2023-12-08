﻿using AtomicSensors.Models;
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

    public async Task<List<SensorData>> GetAsync(string? sort_mode, string? sort_by, int? id_filter, string? type_filter, string? date_filter)
    {
        // Budowanie filtrów
        var filterBuilder = Builders<SensorData>.Filter;
        var sensorIdFilter = (id_filter.HasValue ? filterBuilder.Eq("SensorId", id_filter.Value) : filterBuilder.Empty);
        var sensorTypeFilter = (type_filter != null ? filterBuilder.Eq("SensorType", type_filter) : filterBuilder.Empty);
        var filter = sensorIdFilter & sensorTypeFilter;

        // Budowanie sposobu sortowania
        var sortBuilder = Builders<SensorData>.Sort;
        var sort = sort_by == null ? null : 
            (sort_mode == "asc" ? sortBuilder.Ascending(sort_by) :
            (sort_mode == "desc" ? sortBuilder.Descending(sort_by) : null));

        // Jeśli metoda sortowania nie została podana lub podane zostało
        // tylko jedno z (sort_mode, sort_by), nie dodaje wywołania metody sortowania niżej
        // (Sort() nie przyjmuje null'a i nie widzę w sortBuilder czegoś takiego jak ma filterBuilder.Empty)
        if(sort != null)
            return await _sensorDataCollection.Find<SensorData>(filter != null ? filter : filterBuilder.Empty)
                .Sort(sort)
                .ToListAsync();
        else
            return await _sensorDataCollection.Find<SensorData>(filter != null ? filter : filterBuilder.Empty)
                .ToListAsync();
    }
    public async Task CreateAsync(SensorData sensorData) 
    {
        await _sensorDataCollection.InsertOneAsync(sensorData);
        return;
    }
}
