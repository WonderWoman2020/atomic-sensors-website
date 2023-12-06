using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AtomicSensors
{
    public class SensorData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // ID encji w bazie danych
        public int SensorId { get; set; }
        public string SensorType { get; set; }
        public double Data { get; set; }

        public SensorData(int sensorId, string sensorType, double data)
        {
            SensorId = sensorId;
            SensorType = sensorType;
            Data = data;
        }

        public static SensorData parse(string data)
        {
            var dict =  data.Trim().Split(", ")
                .Select(split => split.Split(": "))
                .Where(pair => pair.Length == 2)
                .ToDictionary(pair => pair[0], pair => pair[1]);

            int id = Convert.ToInt32(dict["ID"]);
            string type = dict["Type"];
            double measuredData = Convert.ToDouble(dict["Data"]); 

            return new SensorData(id, type, measuredData);
        }

        override public string ToString()
        {
            return $"Type: {SensorType}, ID: {SensorId}, Data: {Data}";
        }
    }
}
