using System.Text.Json.Serialization;

namespace NotifierDeliveryWorker.DeliveryWorker.Models
{
    public class StatusUpdatePayload
    {
        public Notification Notification { get; set; }
        public short NewStatusId { get; set; }

        [JsonConstructor]
        public StatusUpdatePayload(Notification notification, short newStatusId)
        {
            this.Notification = notification;
            this.NewStatusId = newStatusId;
        }
    }
}
