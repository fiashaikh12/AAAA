﻿using System;
using System.Collections.Generic;

namespace Entities
{
    public class Cart
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderConfirmation
    {
        public int DistributorId { get; set; }
        public decimal TotalAmount { get; set; }
        public int RetailerId { get; set; }
        public int ProductId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMode { get; set; }
        public int DeliveryTimeSlotId { get; set; }
        public int OrderId { get; set; }
    }
    public class Order
    {
        public List<OrderConfirmation> Request { get; set; }
    }
    public class OrderDeliveryTimeSlot
    {
        public int TimeSlotId { get; set; }
        public string TimeSlotText { get; set; }
    }
}
