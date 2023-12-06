﻿using Microsoft.AspNetCore.Mvc;
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
        }

        public async Task<MqttClientConnectResult> ConnectToMqttBroker()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost")
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(25))
                .Build();

            return await client.ConnectAsync(options, CancellationToken.None);
        }

        public async Task ReceiveMessages()
        {
            var connectResult = ConnectToMqttBroker(); // otwarcie połączenia z kolejką
            if (connectResult.Result.ResultCode == MqttClientConnectResultCode.Success)
            {
                Console.WriteLine("Connection with MQTT queue sucsessful");

                // Ustawienie funkcji callback, jaka ma się wykonać, gdy odebrano wiadomość
                client.ApplicationMessageReceivedAsync += e =>
                {
                    string message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
                    SensorData sensorData = SensorData.parse(message);
                    Console.WriteLine($"Received message: {sensorData}");
                    // Tutaj można potem dopisać zapisywanie wiadomości do bazy danych z użyciem przyszłej klasy repozytorium
                    return Task.CompletedTask;
                };

                // Subskrypcja wszystkich czujników z kolejki
                await client.SubscribeAsync(topic);
            }
            else
                Console.WriteLine("Connection with MQTT queue failed");
        }

        public async Task Stop()
        {
            await client.UnsubscribeAsync(topic);
            await client.DisconnectAsync();
            Console.WriteLine("Connection with MQTT queue stopped");
        }

    }
}
