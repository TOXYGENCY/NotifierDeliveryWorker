namespace NotifierDeliveryWorker.DeliveryWorker.Models
{
    public class StatusUpdatePayload
    {
        public Notification notification { get; set; }
        public short newStatusId { get; set; }

        public StatusUpdatePayload(Notification notification, short newStatusId)
        {
            this.notification = notification;
            this.newStatusId = newStatusId;
        }
    }
}
