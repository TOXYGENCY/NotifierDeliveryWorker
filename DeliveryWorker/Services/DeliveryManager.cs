using NotifierDeliveryWorker.DeliveryWorker.Interfaces;
using NotifierDeliveryWorker.DeliveryWorker.Models;

namespace NotifierDeliveryWorker.DeliveryWorker.Services
{
    public class DeliveryManager : IDeliveryManager
    {
        private readonly IRabbitPublisher rabbitPublisher;

        public DeliveryManager(IRabbitPublisher rabbitPublisher)
        {
            this.rabbitPublisher = rabbitPublisher;
        }

        public async Task DeliverNotificationAsync(Notification notification)
        {
            ArgumentNullException.ThrowIfNull(notification);

            try
            {
                await DeliverAsync();
                await SendStatusSentRabbitAsync(notification);
                Console.WriteLine($" [v] Sent notificaion.");
            }
            catch (Exception)
            {
                Console.WriteLine($" [XXX] ERROR WHILE DELIVERING NOTIFICATION:");
                await SendStatusDeliveryErrorRabbitAsync(notification);
                throw;
            }
        }

        private async Task DeliverAsync()
        {
            // Имитация задержки обработки уведомлений
            await Task.Delay(new Random().Next(100, 1500));
            // Имитация ошибки при отправке уведомлений
            if (new Random().Next(5+1) == 5) throw new Exception("FAKE DELIVERY ERROR");
        }

        // TODO: статус должен браться из Redis
        private async Task SendStatusRabbitAsync(Notification notification, short newStatusId)
        {
            var payload = new StatusUpdatePayload(notification, newStatusId);
            await rabbitPublisher.PublishAsync(payload);
        }

        private async Task SendStatusDeliveryErrorRabbitAsync(Notification notification)
        {
            await SendStatusRabbitAsync(notification, (short)StatusesEnum.DELIVERY_ERROR); // TODO
        }

        private async Task SendStatusSentRabbitAsync(Notification notification)
        {
            await SendStatusRabbitAsync(notification, (short)StatusesEnum.SENT); // TODO
        }
    }
}
