using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System.Text;

namespace AtomicSensors
{
    public class QueueService
    {
        private IMqttClient client;
        private string topic = "sensor/+"; // Plus oznacza subskrybcję 1 poziomu niżej o jakiejkolwiek nazwie

        public QueueService()
        {
            var factory = new MqttFactory();
            client = factory.CreateMqttClient();

            ConnectToMqttBroker().Wait();
            Console.WriteLine("Connection with MQTT queue sucsessful");
        }

        private async Task ConnectToMqttBroker()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost")
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(25))
                .Build();

            await client.ConnectAsync(options, CancellationToken.None);
        }

        public async Task ReceiveMessages()
        {
            // Callback function when a message is received
            client.ApplicationMessageReceivedAsync += e =>
            {
                //Console.WriteLine($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
                SensorData sensorData = SensorData.parse(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment));
                Console.WriteLine("Received message: "+sensorData.ToString());
                // Tutaj można potem dopisać zapisywanie wiadomości do bazy danych z użyciem przyszłej klasy repozytorium
                return Task.CompletedTask;
            };

            // Subscribe to a topic
            await client.SubscribeAsync(topic);
        }

    }
}
