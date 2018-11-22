using Entities;

namespace Interface
{
    public interface IDashboard
    {
        ServiceRes NearByDistributors(NearByDistributors nearBy);
        ServiceRes CategoryListByDistributor(NearByDistributors byDistributors);
        ServiceRes ProductByCategories(NearByDistributors nearBy);
    }
}
