
using Microsoft.Extensions.DependencyInjection;
using NotifierDeliveryWorker.DeliveryWorker.Infrastructure;
using NotifierNotificationService.NotificationService.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

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
