using System;
using System.Threading;
using System.Threading.Tasks;

namespace czujniki_elektrownia
{
    class Program
    {
        static void Main(string[] args)
        {
            var manualDataInput = new ManualDataInput();
            bool start = false;

            while (start==false)
            {
                Console.WriteLine("Wybierz opcję:");
                Console.WriteLine("1. Podaj dane z klawiatury");
                Console.WriteLine("2. Uruchom wysyłanie danych przez wątki");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        manualDataInput.EnterDataOnce();
                        break;
                    case "2":
                        start = true;
                        StartSensorThreads();
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        break;
                }
            }
        }
        // Metoda do uruchamiania wątków z opóźnieniem


        static void StartSensorThreads()
        {
            var manualDataInput = new ManualDataInput();
            new Thread(manualDataInput.Run).Start();

            for (int i = 0; i < 4; i++)
            {
                int sensorId = i; // Lokalna zmienna przechowująca wartość i

                var tempSensor = new TemperatureSensor(sensorId);
                new Thread(tempSensor.Run).Start();
                Thread.Sleep(2000);

                var pressureSensor = new PressureSensor(sensorId);
                new Thread(pressureSensor.Run).Start();
                Thread.Sleep(2000);

                var seismometer = new Seismometer(sensorId);
                new Thread(seismometer.Run).Start();
                Thread.Sleep(2000);

                var radiationSensor = new RadiationSensor(sensorId);
                new Thread(radiationSensor.Run).Start();
                Thread.Sleep(2000);
            }
        }

    }
}
