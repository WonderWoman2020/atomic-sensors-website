using System;
using System.Threading;
using System.Threading.Tasks;

namespace czujniki_elektrownia
{
    class Program
    {
        static void Main(string[] args)
        {
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
