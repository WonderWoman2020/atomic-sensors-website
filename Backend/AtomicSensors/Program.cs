using MQTTnet.Client;

namespace AtomicSensors
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QueueService queue = new QueueService(); 
            queue.ReceiveMessages();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton(queue); // Dodanie naszego serwisu do obs�ugi kolejki do kontera aplikacji jako obiekt �yj�cy ca�� aplikacj�

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
