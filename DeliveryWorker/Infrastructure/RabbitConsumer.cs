using NotifierDeliveryWorker.DeliveryWorker.Models;
using NotifierDeliveryWorker.DeliveryWorker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotifierDeliveryWorker.DeliveryWorker.Infrastructure
{
    public class RabbitConsumer : BackgroundService
    {
        private readonly IConfiguration config;
        private readonly IDeliveryManager deliveryManager;
        private string hostname;
        private string username;
        private string password;
        private string queue;
        private ConnectionFactory connectionFactory;
        private IChannel channel;
        private IConnection connection;
        private AsyncEventingBasicConsumer consumer;

        public RabbitConsumer(IConfiguration configuration, IDeliveryManager deliveryManager)
        {
            this.config = configuration;
            hostname = config["RabbitMq:HostName"] ?? "rabbitmq";
            username = config["RabbitMq:UserName"] ?? "admin";
            password = config["RabbitMq:Password"] ?? "admin";
            queue = config["RabbitMq:NotificationsQueueName"] ?? "notifications";
            this.deliveryManager = deliveryManager;
        }

        private async Task Init()
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };

            connection = await connectionFactory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();
            consumer = new AsyncEventingBasicConsumer(channel);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Инициализация переменных соединений, каналов и тд
            await Init();

            // Идемпотентное создание очереди (существует или нет - будет существовать)
            await channel.QueueDeclareAsync(queue: queue, durable: false,
                exclusive: false, autoDelete: false, arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            // Подписываемся на событие получения сообщения
            consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                // Обрабатываем полученное сообщение
                await ProcessMessageAsync(eventArgs);
            };

            // Начинаем потребление сообщений из очереди
            await channel.BasicConsumeAsync(queue, autoAck: true, consumer: consumer);

            // Бесконечная работа:
            while (!stoppingToken.IsCancellationRequested)
                await Task.Delay(1000, stoppingToken);
        }

        private async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs)
        {
            var body = eventArgs.Body.ToArray(); // byte[]
            var message = Encoding.UTF8.GetString(body); // string

            Console.WriteLine($" [v] Received {message}");

            var notification = JsonSerializer.Deserialize<Notification>(message);
            await deliveryManager.DeliverNotificationAsync(notification);
        }
    }
}
