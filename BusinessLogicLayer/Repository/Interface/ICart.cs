using Entities;

namespace Interface
{
    public interface ICart
    {
        ServiceRes AddToCart(Cart cart);
        ServiceRes DeleteItemfromCart(Cart cart);
        ServiceRes OrderConfirmation(OrderConfirmation orderConfirmation);
        ServiceRes ViewCartItem();
    }
}
