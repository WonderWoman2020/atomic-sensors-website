using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class ManualDataInput
{
    private IMqttClient client;
    //public int SensorId { get; private set; }

    public ManualDataInput()//int sensorId
    {
        //SensorId = sensorId;

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

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("Wprowadź typ czujnika (pressure, seismometer, radiation, temperature): ");
            string sensorType = Console.ReadLine();
            Console.WriteLine("Wprowadź ID czujnika (0, 1, 2, 3): ");
            int sensorId = int.Parse(Console.ReadLine());
            Console.WriteLine("Wprowadź wartość: ");
            double value = double.Parse(Console.ReadLine());

            var currentTime = DateTime.Now;
            var timestamp = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string topic = $"sensor/{sensorType}";
            string message = $"Type: {char.ToUpper(sensorType[0]) + sensorType.Substring(1)}, ID: {sensorId}, Data: {value}, Time: {timestamp}";
            var messagePayload = Encoding.UTF8.GetBytes(message);

            var mqttMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(messagePayload)
                    .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce)
                    .WithRetainFlag()
                    .Build();

            client.PublishAsync(mqttMessage, CancellationToken.None).Wait();
        }
    }
}