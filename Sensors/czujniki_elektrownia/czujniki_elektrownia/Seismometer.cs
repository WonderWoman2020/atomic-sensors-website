using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace czujniki_elektrownia
{
    class Seismometer
    {
        public int SensorId { get; private set; }
        private Random random = new Random();

        private IMqttClient client;
        private string topic = "sensor/seismometer";

        // Zakresy magnitudy
        private const double NormalUpperLimit = 3.0; // Normalna magnituda maksymalna
        private const double WarningLowerLimit = 3.1; // Początek zakresu ostrzeżenia
        private const double WarningUpperLimit = 4.5; // Koniec zakresu ostrzeżenia
        private const double AlarmLowerLimit = 4.6; // Początek zakresu alarmowego

        public Seismometer(int sensorId)
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
            double magnitude = GetRandomMagnitude();
            magnitude = Math.Round(magnitude, 6);

            return magnitude;
        }

        private double GetRandomMagnitude()
        {
            if (random.NextDouble() < 0.8) // 80% szans na normalną magnitudę
            {
                return random.NextDouble() * (NormalUpperLimit - 1) + 1; // Zakładając, że 1 jest dolną granicą normalnych wartości
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

                    var currentTime = DateTime.Now;
                    var timestamp = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var message = $"Type= Seismometer; ID= {SensorId}; Data= {data}; Time= {timestamp}";
                    var messagePayload = Encoding.UTF8.GetBytes(message);

                    var mqttMessage = new MqttApplicationMessageBuilder()
                            .WithTopic(topic)
                            .WithPayload(messagePayload)
                            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                            .WithRetainFlag()
                            .Build();

                    client.PublishAsync(mqttMessage, CancellationToken.None);

                    //Console.WriteLine(message);
                    Thread.Sleep(37000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                }
            }
        }
    }
}
