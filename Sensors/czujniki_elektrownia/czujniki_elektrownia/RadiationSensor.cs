using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace czujniki_elektrownia
{
    class RadiationSensor
    {
        public int SensorId { get; private set; }
        private Random random = new Random();

        private IMqttClient client;
        private string topic = "sensor/radiation";

        private const double NormalUpperLimit = 0.5; // Normalny poziom maksymalny
        private const double WarningLowerLimit = 0.6; // Początek zakresu ostrzeżenia
        private const double WarningUpperLimit = 2.0; // Koniec zakresu ostrzeżenia
        private const double AlarmLowerLimit = 2.1; // Początek zakresu alarmowego

        public RadiationSensor(int sensorId)
        {
            SensorId = sensorId;

            var factory = new MqttFactory();
            client = factory.CreateMqttClient();

            ConnectToMqttBroker().Wait();
        }

        private async Task ConnectToMqttBroker()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost")
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(25))
                .Build();

            await client.ConnectAsync(options, CancellationToken.None);
        }

        public double GenerateData()
        {
            double radiationLevel = GetRandomRadiationLevel();
            radiationLevel = Math.Round(radiationLevel, 6);

            return radiationLevel;
        }

        private double GetRandomRadiationLevel()
        {
            if (random.NextDouble() < 0.8) // 80% szans na normalny poziom radiacji
            {
                return random.NextDouble() * (NormalUpperLimit - 0.1) + 0.1; // Zakładając, że 0.1 jest dolną granicą normalnych wartości
            }
            else if (random.NextDouble() < 0.95) // 15% szans na wartość ostrzeżenia
            {
                return random.NextDouble() * (WarningUpperLimit - WarningLowerLimit) + WarningLowerLimit;
            }
            else // 5% szans na wartość alarmową
            {
                return random.NextDouble() * (AlarmLowerLimit - WarningUpperLimit) + WarningUpperLimit;
            }
        }
        public void Run()
        {
            while (true)
            {
                try
                {
                    var data = GenerateData();
                    var message = $"Type: Radiation, ID: {SensorId}, Data: {data}";
                    var messagePayload = Encoding.UTF8.GetBytes(message);
                    var mqttMessage = new MqttApplicationMessageBuilder()
                            .WithTopic(topic)
                            .WithPayload(messagePayload)
                            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                            .WithRetainFlag()
                            .Build();

                    client.PublishAsync(mqttMessage, CancellationToken.None);

                    Console.WriteLine(message);
                    Thread.Sleep(15000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                }
            }
        }
    }
}
