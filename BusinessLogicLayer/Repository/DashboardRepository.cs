using Entities;
using Interface;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class DashboardRepository : IDashboard
    {
        public ServiceRes NearByDistributors()
        {
            ServiceRes serviceRes = new ServiceRes();
            try {

            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }
    }
}
