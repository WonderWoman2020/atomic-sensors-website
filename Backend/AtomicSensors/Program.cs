using AtomicSensors.Models;
using AtomicSensors.Services;
using MQTTnet.Client;

namespace AtomicSensors
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Konfiguracja i dodanie serwisu do obs�ugi bazy danych
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddSingleton<MongoDBService>();

            // Dodanie naszego serwisu do obs�ugi kolejki do kontera aplikacji jako obiekt �yj�cy ca�� aplikacj�
            builder.Services.AddSingleton<QueueService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Pobranie beana serwisu kolejki z kontenera aplikacji, �eby m�c uruchomi� jej metod� odbioru wiadomo�ci
            var queue = app.Services.GetService<QueueService>();
            queue.ReceiveMessages();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            // TODO Wygl�da na to, �e tutaj nie dociera nawet jak si� przegl�dark� zamknie
            // odbi�r wiadomo�ci i tak si� chyba sam zamyka jak si� program wy��cza, ale dla porz�dku mo�na potem spr�bowa� poprawi�
            //queue.Stop().Wait();
        }
    }
}
