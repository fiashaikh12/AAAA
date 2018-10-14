using BusinessObjects.Entities;
using DataAccessLayer;
using Entities;
using Interface;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Repository
{
    public class DashboardRepository : IDashboard
    {
        public ServiceRes NearByDistributors(NearByDistributors request)
        {
            ServiceRes<List<NearByDistributors>> serviceRes = new ServiceRes<List<NearByDistributors>>();
            try {
                SqlParameter [] sqlParameter = new SqlParameter[2];
                sqlParameter[0] = new SqlParameter { ParameterName = "@Member_Id", Value = request.UserId };
                sqlParameter[1] = new SqlParameter { ParameterName = "@flag", Value = "ND" };
                DataTable dtNearbyDist = SqlHelper.GetTableFromSP("Usp_Products_Test", sqlParameter);
                if(dtNearbyDist.Rows.Count > 0)
                {
                    List<NearByDistributors> nearByDistributors = dtNearbyDist.AsEnumerable().
                        Select(x => new NearByDistributors {
                            CompanyName=x.Field<string>("Company_Name"),
                            Distance=x.Field<double>("Distance"),
                            UserId =x.Field<int>("Member_Id")
                        }).ToList();
                    serviceRes.Data = nearByDistributors;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Near by distributor";
                }
                else
                {
                    serviceRes.Data = null;
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Distributor not found";
                }
            }
            catch(Exception ex) {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }
    }
}
