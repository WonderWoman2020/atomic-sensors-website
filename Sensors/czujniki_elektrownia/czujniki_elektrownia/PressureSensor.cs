using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace czujniki_elektrownia
{
    class PressureSensor
    {

        public int SensorId { get; private set; }
        private Random random = new Random();

        private IMqttClient client;
        private string topic = "sensor/pressure";

        private const double NormalUpperLimit = 100; // Normalne ciśnienie maksymalne
        private const double WarningLowerLimit = 101; // Początek zakresu ostrzeżenia
        private const double WarningUpperLimit = 150; // Koniec zakresu ostrzeżenia
        private const double AlarmLowerLimit = 151; // Początek zakresu alarmowego

        public PressureSensor(int sensorId)
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
            double pressure = GetRandomPressure();
            pressure = Math.Round(pressure, 4);

            return pressure;
        }

        private double GetRandomPressure()
        {
            if (random.NextDouble() < 0.8) // 80% szans na normalne ciśnienie
            {
                return random.NextDouble() * (NormalUpperLimit - 20) + 20; // Zakładając, że 20 jest dolną granicą normalnych wartości
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
                    var message = $"Type: Pressure, ID: {SensorId}, Data: {data}";
                    var messagePayload = Encoding.UTF8.GetBytes(message);

                    var mqttMessage = new MqttApplicationMessageBuilder()
                            .WithTopic(topic)
                            .WithPayload(messagePayload)
                            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                            .WithRetainFlag()
                            .Build();

                    client.PublishAsync(mqttMessage, CancellationToken.None);

                    Console.WriteLine(message);
                    Thread.Sleep(30000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                }
            }
        }
    }
}
