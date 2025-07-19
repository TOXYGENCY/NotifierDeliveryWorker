using NotifierDeliveryWorker.DeliveryWorker.Infrastructure;
using NotifierDeliveryWorker.DeliveryWorker.Interfaces;
using NotifierDeliveryWorker.DeliveryWorker.Services;

namespace NotifierDeliveryWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHostedService<RabbitConsumer>();
            builder.Services.AddTransient<IDeliveryManager, DeliveryManager>();
            builder.Services.AddTransient<IRabbitPublisher, RabbitPublisher>();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            app.Urls.Add("http://*:6122");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
