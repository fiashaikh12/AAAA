using System;

namespace Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderConfirmation:Cart
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public int DeliveryTimeSlotId { get; set; }
        public int OrderId { get; set; }
    }
    public class OrderDeliveryTimeSlot
    {
        public int TimeSlotId { get; set; }
        public string TimeSlotText { get; set; }
    }
}
