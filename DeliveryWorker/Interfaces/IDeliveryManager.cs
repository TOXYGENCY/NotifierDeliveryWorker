using NotifierDeliveryWorker.DeliveryWorker.Models;

namespace NotifierDeliveryWorker.DeliveryWorker.Services
{
    public interface IDeliveryManager
    {
        Task DeliverNotificationAsync(Notification notification);
    }
}
