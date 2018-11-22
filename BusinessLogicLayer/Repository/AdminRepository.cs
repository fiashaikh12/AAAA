using DataAccessLayer;
using Entities;
using System;
using System.Data.SqlClient;

namespace Repository
{
    public class AdminRepository : IAdmin
    {
        public ServiceRes AddCategory(string name)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter { ParameterName = "@value", Value = name };
                parameter[1] = new SqlParameter { ParameterName = "@flag", Value = "A" };
                int ret = SqlHelper.ExecuteNonQuery("Usp_Admin_CatgoryMaster", parameter);
                if(ret> 0)
                {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Success";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Failed";
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes AddSubCategory(string name, int categoryId)
        {
            throw new NotImplementedException();
        }

        public ServiceRes DeleteCategory(int id)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter { ParameterName = "@id", Value = id };
                parameter[1] = new SqlParameter { ParameterName = "@flag", Value = "D" };
                int ret = SqlHelper.ExecuteNonQuery("Usp_Admin_CatgoryMaster", parameter);
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes DeleteSubCategory(int subCategoryId)
        {
            throw new NotImplementedException();
        }

        public ServiceRes IsAdminValid(AdminRequest adminRequest)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] parameter = new SqlParameter[2];
                parameter[0] = new SqlParameter { ParameterName = "@username", Value = adminRequest.Username };
                parameter[1] = new SqlParameter { ParameterName = "@password", Value = adminRequest.Password };
                var adminDt = SqlHelper.GetTableFromSP("Usp_AdminLogin", parameter);
                if(adminDt.Rows.Count > 0)
                {
                    if (Convert.ToString(adminDt.Rows[0][0]) == "0")
                    {
                        serviceRes.IsSuccess = true;
                        serviceRes.ReturnCode = "200";
                        serviceRes.ReturnMsg = "Success";
                    }
                    else
                    {
                        serviceRes.IsSuccess = false;
                        serviceRes.ReturnCode = "400";
                        serviceRes.ReturnMsg = "Invalid username and password";
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes UpdateCategory(int id, string name)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] parameter = new SqlParameter[3];
                parameter[0] = new SqlParameter { ParameterName = "@id", Value = id };
                parameter[0] = new SqlParameter { ParameterName = "@id", Value = name };
                parameter[1] = new SqlParameter { ParameterName = "@flag", Value = "U" };
                int ret = SqlHelper.ExecuteNonQuery("Usp_Admin_CatgoryMaster", parameter);
                if(ret > 0)
                {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Updated";
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes UpdateSubCategory(string name, int subCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
