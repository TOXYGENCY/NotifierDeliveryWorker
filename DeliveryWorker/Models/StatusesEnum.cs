namespace NotifierDeliveryWorker.DeliveryWorker.Models
{
	public enum StatusesEnum : short
	{
		CREATED = 1,
        PENDING_SEND = 2,
		SENT = 3,
		ERROR = 4,
        CREATION_ERROR = 5,
        UPDATE_ERROR = 6,
		DELIVERY_ERROR = 7,
	}
}
