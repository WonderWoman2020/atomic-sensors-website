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
                        //new Thread(manualDataInput.Run).Start(); // Uruchomienie wątku do ciągłego wprowadzania danych
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór, spróbuj ponownie.");
                        break;
                }
            }
        }
        static void StartSensorThreads()
        {
            var manualDataInput = new ManualDataInput();
            new Thread(manualDataInput.Run).Start();

            for (int i = 0; i < 4; i++)
            {
                var tempSensor = new TemperatureSensor(i);
                var pressureSensor = new PressureSensor(i);
                var seismometer = new Seismometer(i);
                var radiationSensor = new RadiationSensor(i);

                new Thread(tempSensor.Run).Start();
                Thread.Sleep(1000);
                new Thread(pressureSensor.Run).Start();
                Thread.Sleep(1000);
                new Thread(seismometer.Run).Start();
                Thread.Sleep(1000);
                new Thread(radiationSensor.Run).Start();
                Thread.Sleep(3000);
            }
        }
    }
}
