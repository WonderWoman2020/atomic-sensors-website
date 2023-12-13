using AtomicSensors.Models;
using AtomicSensors.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

            // Konfiguracja tego, �eby Swagger generowa� interaktywn� dokumentacj� z komentarzy w kodzie
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Atomic sensors API",
                    Description = "Proste API w ASP.NET na potrzeby projektu z SI.NET.",
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

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

            app.UseCors(x => x
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader());

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
