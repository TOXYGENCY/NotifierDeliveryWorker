using NotifierNotificationService.NotificationService.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Unicode;

namespace NotifierDeliveryWorker.DeliveryWorker.Infrastructure
{
    public class RabbitConsumer : BackgroundService
    {
        private readonly IConfiguration configuration;

        public RabbitConsumer(IConfiguration configuration)
        {
            this.configuration = configuration;
        }   

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connfact = new ConnectionFactory
            {
                HostName = configuration["RabbitMq:HostName"],
                UserName = configuration["RabbitMq:UserName"],
                Password = configuration["RabbitMq:Password"],
            };

            using var conn = await connfact.CreateConnectionAsync();
            using var channel = await conn.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "hello world", durable: false,
                exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync("hello world", autoAck: true, consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
