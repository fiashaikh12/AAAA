using System;
using System.Collections.Generic;
using Entities;
using System.Data.SqlClient;
using DataAccessLayer;
using System.Data;
using System.Web.Hosting;
using System.IO;
using System.Linq;

namespace Repository
{
    public class CommonRepository : ICommonRepository
    {
        public ServiceRes GetBusinessType()
        {
            ServiceRes<List<BusinessTypeCategory>> serviceRes = new ServiceRes<List<BusinessTypeCategory>>();
            try
            {
                DataTable dtBusinessMaster = SqlHelper.GetTableFromSP("Usp_GetBusinessMaster");
                if (dtBusinessMaster.Rows.Count > 0)
                {
                    List<BusinessTypeCategory> businessCategory = dtBusinessMaster.AsEnumerable().
                            Select(x => new BusinessTypeCategory
                            {
                                BusinessId = x.Field<int>("Business_Id"),
                                BusinessName = x.Field<string>("Name")
                            }).ToList();
                    serviceRes.Data = businessCategory;
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Business master";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
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
                LogManager.WriteLog(ex);
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
                LogManager.WriteLog(ex);
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
                LogManager.WriteLog(ex);
                serviceRes.Data = null;
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Something went wrong";
            }
            return serviceRes;
        }

        public string Base64toImage(string base64string, string directory, string subdirectory,string fileName)
        {
            string filelocation = String.Empty;
            if (!String.IsNullOrEmpty(base64string))
            {
                try
                {
                    //bool exists = Directory.Exists(HostingEnvironment.MapPath("~/"+ directory));

                    if (!Directory.Exists(HostingEnvironment.MapPath("~/" + directory)))
                        Directory.CreateDirectory(HostingEnvironment.MapPath("~/"+ directory));

                    //exists = Directory.Exists(HostingEnvironment.MapPath("~/Images/" + subdirectory));

                    if (!Directory.Exists(HostingEnvironment.MapPath($"~/{directory}/" + subdirectory)))
                        Directory.CreateDirectory(HostingEnvironment.MapPath($"~/{directory}/" + subdirectory));

                    string imageformat = String.Empty;
                    var data = base64string.Substring(0, 5);
                    switch (data.ToUpper())
                    {
                        case "IVBOR": imageformat = ".png"; break;
                        case "/9J/4": imageformat = ".jpeg"; break;
                        case "AAAAF": imageformat = ".mp4"; break;
                        case "JVBER": imageformat = ".pdf"; break;
                        case "R0lGO":imageformat = ".gif";break;
                        default: imageformat = ""; break;
                    }


                    //Convert Base64 Encoded string to Byte Array.
                    byte[] imageBytes = Convert.FromBase64String(base64string);

                    string filename = $"{fileName}_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}";
                    //Save the Byte Array as Image File.
                    filelocation = $"{directory}/{subdirectory}/" + filename + imageformat;
                    string filePath = Path.Combine(HostingEnvironment.MapPath($"~/{directory}/{subdirectory}/{filename}{imageformat}"));
                    File.WriteAllBytes(filePath, imageBytes);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(ex);
                }

            }

            return filelocation;
        }

        public bool IsBase64Valid(string base64String)
        {
            throw new NotImplementedException();
        }
    }
}
