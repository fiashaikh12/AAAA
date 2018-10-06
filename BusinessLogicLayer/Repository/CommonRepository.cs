using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Data;
using BusinessLogicLayer.Repository;
using static Enum.Enums;

namespace Repository
{
    public class CommonRepository : ICommonRepository
    {
        public ServiceRes GetBusinessType()
        {
            ServiceRes<List<BusinessTypeCategory>> serviceRes = new ServiceRes<List<BusinessTypeCategory>>();
            try
            {
                List<BusinessTypeCategory> businesses = new List<BusinessTypeCategory>();
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetCategory");
                foreach (DataRow row in dtCities.Rows)
                {
                    BusinessTypeCategory businessCategory = new BusinessTypeCategory
                    {
                        BusinessId = Convert.ToInt32(row["Product_Category_Id"]),
                        BusinessName = Convert.ToString(row["Product_Name"])
                    };
                    businesses.Add(businessCategory);
                }
                serviceRes.Data = businesses;
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "Category master";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
                serviceRes.Data = null;
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Something went wrong";
            }
            return serviceRes;
        }

        public ServiceRes GetCitiesByState(States states)
        {
            ServiceRes<List<Cities>> serviceRes = new ServiceRes<List<Cities>>();
            try
            {
                List<Cities> cities = new List<Cities>();
                SqlParameter[] sqlParameter = new SqlParameter[1];
                sqlParameter[0] = new SqlParameter { ParameterName = "@StateId", Value = states.StateId };
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetCitiesByState", sqlParameter);
                foreach (DataRow row in dtCities.Rows)
                {
                    Cities city = new Cities
                    {
                        CityId = Convert.ToInt32(row["CityId"]),
                        City = Convert.ToString(row["City"])
                    };
                    cities.Add(city);
                }
                serviceRes.Data = cities;
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
                serviceRes.Data = null;
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Something went wrong";
            }
            return serviceRes;
        }

        public ServiceRes GetGenders()
        {
            ServiceRes<List<Genders>> serviceRes = new ServiceRes<List<Genders>>();
            try
            {
                List<Genders> genders = new List<Genders>();
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetGender");
                foreach (DataRow row in dtCities.Rows)
                {
                    Genders gender = new Genders
                    {
                        GenderId = Convert.ToInt32(row["GenderId"]),
                        Gender = Convert.ToString(row["Gender"])
                    };
                    genders.Add(gender);
                }
                serviceRes.Data = genders;
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
                serviceRes.Data = null;
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Something went wrong";
            }
            return serviceRes;
        }

        public ServiceRes GetStates()
        {
            ServiceRes<List<States>> serviceRes = new ServiceRes<List<States>>();
            try
            {
                List<States> states = new List<States>();
                DataTable dtCities = SqlHelper.GetTableFromSP("Usp_GetStates");
                foreach (DataRow row in dtCities.Rows)
                {
                    States state = new States
                    {
                        StateId = Convert.ToInt32(row["StateId"]),
                        State = Convert.ToString(row["State"])
                    };
                    states.Add(state);
                }
                serviceRes.Data = states;
                serviceRes.IsSuccess = true;
                serviceRes.ReturnCode = "200";
                serviceRes.ReturnMsg = "";
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
                serviceRes.Data = null;
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Something went wrong";
            }
            return serviceRes;
        }

    }
}
