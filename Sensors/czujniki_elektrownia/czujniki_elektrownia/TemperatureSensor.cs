using MQTTnet;
using MQTTnet.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace czujniki_elektrownia
{
    class TemperatureSensor
    {
        public int SensorId { get; private set; }
        private Random random = new Random();

        private IMqttClient client;
        private string topic = "sensor/temperature";

        private const double NormalUpperLimit = 350; // Normalna temperatura maksymalna
        private const double WarningLowerLimit = 351; // Początek zakresu ostrzeżenia
        private const double WarningUpperLimit = 450; // Koniec zakresu ostrzeżenia
        private const double AlarmLowerLimit = 451; // Początek zakresu alarmowego

        public TemperatureSensor(int sensorId)
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
            double temperature = GetRandomTemperature();
            temperature = Math.Round(temperature, 3);

            return temperature;
        }

        private double GetRandomTemperature()
        {
            if (random.NextDouble() < 0.8) // 80% szans na normalną wartość
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

                    var currentTime = DateTime.Now;
                    var timestamp = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    
                    var message = $"Type= Temperature; ID= {SensorId}; Data= {data}; Time= {timestamp}";
                    var messagePayload = Encoding.UTF8.GetBytes(message);

                    var mqttMessage = new MqttApplicationMessageBuilder()
                            .WithTopic(topic)
                            .WithPayload(messagePayload)
                            .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                            .WithRetainFlag()
                            .Build();

                    client.PublishAsync(mqttMessage, CancellationToken.None).Wait();

                    //Console.WriteLine(message);
                    Thread.Sleep(13000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                }
            }
        }
    }
}