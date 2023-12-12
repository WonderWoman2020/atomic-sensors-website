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

            // Konfiguracja i dodanie serwisu do obs³ugi bazy danych
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddSingleton<MongoDBService>();

            // Dodanie naszego serwisu do obs³ugi kolejki do kontera aplikacji jako obiekt ¿yj¹cy ca³¹ aplikacjê
            builder.Services.AddSingleton<QueueService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Konfiguracja tego, ¿eby Swagger generowa³ interaktywn¹ dokumentacjê z komentarzy w kodzie
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

            // Pobranie beana serwisu kolejki z kontenera aplikacji, ¿eby móc uruchomiæ jej metodê odbioru wiadomoœci
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

            // TODO Wygl¹da na to, ¿e tutaj nie dociera nawet jak siê przegl¹darkê zamknie
            // odbiór wiadomoœci i tak siê chyba sam zamyka jak siê program wy³¹cza, ale dla porz¹dku mo¿na potem spróbowaæ poprawiæ
            //queue.Stop().Wait();
        }
    }
}
