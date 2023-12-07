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

            // Konfiguracja i dodanie serwisu do obs³ugi bazy danych
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddSingleton<MongoDBService>();

            // Dodanie naszego serwisu do obs³ugi kolejki do kontera aplikacji jako obiekt ¿yj¹cy ca³¹ aplikacjê
            builder.Services.AddSingleton<QueueService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Pobranie beana serwisu kolejki z kontenera aplikacji, ¿eby móc uruchomiæ jej metodê odbioru wiadomoœci
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

            // TODO Wygl¹da na to, ¿e tutaj nie dociera nawet jak siê przegl¹darkê zamknie
            // odbiór wiadomoœci i tak siê chyba sam zamyka jak siê program wy³¹cza, ale dla porz¹dku mo¿na potem spróbowaæ poprawiæ
            //queue.Stop().Wait();
        }
    }
}
