﻿using Entities;
using System.Collections.Generic;

namespace Interface
{
    public interface ICart
    {
        ServiceRes AddToCart(Cart cart);
        ServiceRes DeleteItemfromCart(Cart cart);
        ServiceRes OrderConfirmation(Order orders);
        ServiceRes ViewCartItem(Cart cart);
        ServiceRes DeliveryTimeSlot();
    }
}
