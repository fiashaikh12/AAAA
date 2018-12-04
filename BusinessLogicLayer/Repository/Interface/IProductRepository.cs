using Entities;

namespace Repository
{
    public interface IProductRepository
    {
        ServiceRes Distributor_Report(Distributor_User distributor_User);
        ServiceRes AddProduct(ProductDetails objProduct);
        ServiceRes DeleteProduct(int productId);
        ServiceRes UpdateProduct(ProductDetails objProduct);
        ServiceRes ViewProducts(Distributor_User distributor_User);
        //ServiceRes GetProductDetailById();
        ServiceRes GetCategoryMaster();
        ServiceRes GetSubCategoryMaster(int categoryId);
        ServiceRes GetAllRecentOrders(RecentReport recentReport);
        ServiceRes RecentOrderDetail(RecentReport  recentReport);
        ServiceRes Distributor_ConfirmOrder(ConfirmOrder confirmOrder);
    }
}
