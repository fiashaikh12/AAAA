using BusinessObjects.Entities;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IDashboard
    {
        ServiceRes NearByDistributors(NearByDistributors nearBy);
        ServiceRes CategoryListByDistributor(NearByDistributors byDistributors);
        ServiceRes ProductByCategories(NearByDistributors nearBy);
    }
}
