namespace NotifierNotificationService.NotificationService.Domain.Interfaces
{
    public interface IRabbitConsumer
    {
        Task ExecuteAsync<T>(T message, string queue);
    }
}
