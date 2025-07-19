namespace NotifierDeliveryWorker.DeliveryWorker.Models;


public partial class Notification
{

    public Guid RecipientUserId { get; set; }

    public Guid SenderUserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

}
