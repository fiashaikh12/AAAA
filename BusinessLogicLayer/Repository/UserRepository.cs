using System;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Data;
using Entities;
using static Enum.Enums;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private CryptographyRepository _objRepo = CryptographyRepository.GetInstance;

        public UserRepository() { }

        public ServiceRes ChangePassword(User user)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = user.MobileNumber };
                sqlParameters[1] = new SqlParameter { ParameterName = "@MobileNumber", Value = user.Password };

                int returnValue = SqlHelper.ExecuteNonQuery("Usp_UpdatePassword", sqlParameters);
                if (returnValue == 1) {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Your password has been changed successfully";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "201";
                    serviceRes.ReturnMsg = "UnAuthorized attempt";
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
            }
            return serviceRes;
        }
        
        public ServiceRes IsUserValid(User objUser)
        {
            ServiceRes<string> serviceRes = new ServiceRes<string>();
            try
            {
                TokenRepository tokenRepository = TokenRepository.GetInstance;
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = objUser.MobileNumber };

                var dtUser = SqlHelper.GetTableFromSP("Usp_ValidateUser", parameter);
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    if (Convert.ToString(dtUser.Rows[0][0]) == "201")
                    {
                        serviceRes.IsSuccess = false;
                        serviceRes.ReturnCode = "201";
                        serviceRes.ReturnMsg = "Mobile number does not exist.Please register again";
                    }
                    else
                    {
                        string decryptedPassword = _objRepo.Decrypt(Convert.ToString(dtUser.Rows[0][1]));
                        string username = Convert.ToString(dtUser.Rows[0][0]);
                        bool isLocked = Convert.ToBoolean(dtUser.Rows[0][2]);
                        int loginAttempts = Convert.ToInt32(dtUser.Rows[0][3]);
                        bool isLogedIn = Convert.ToBoolean(dtUser.Rows[0][4]);

                        if ((username.Equals(objUser.MobileNumber) && decryptedPassword.Equals(objUser.Password))) {
                            if (!isLocked) {
                                //reset login attempts and is locked column
                                LoginDetails loginDetails = new LoginDetails()
                                {
                                    MobileNumber = objUser.MobileNumber,
                                    IsLocked = false,
                                    LoginAttempts = 3,
                                    IsLogedIn = true
                                };
                                UpdateLoginDetails(loginDetails);
                                serviceRes.IsSuccess = true;
                                serviceRes.ReturnCode = "200";
                                serviceRes.ReturnMsg = "Username verified";
                                serviceRes.Data = tokenRepository.GenerateToken();
                            }
                            else {
                                //return account suspended status
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "403";
                                serviceRes.ReturnMsg = $"Account suspended for user {objUser.MobileNumber}Please contact your administrator";
                            }
                        }
                        else if (String.IsNullOrEmpty(objUser.Password) || decryptedPassword != objUser.Password)
                        {
                            //decrese login attempts
                            //if attempts greater than zero set account status active and decrease attempts
                            loginAttempts = loginAttempts - 1;
                            if (loginAttempts > 0)
                            {
                                LoginDetails loginDetails = new LoginDetails()
                                {
                                    MobileNumber = objUser.MobileNumber,
                                    IsLocked = false,
                                    LoginAttempts = loginAttempts,
                                    IsLogedIn = false
                                };
                                UpdateLoginDetails(loginDetails);
                                //update attempts left and return status as wrong password
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "403";
                                serviceRes.ReturnMsg = $"Wrong Password attempts left {loginAttempts} ";
                            }
                            else
                            {
                                //LoginDetails loginDetails = new LoginDetails()
                                //{
                                //    Username = objUser.MobileNumber,
                                //    IsLocked = true,
                                //    LoginAttempts = 0
                                //};
                                //UpdateLoginDetails(loginDetails);
                                //update if attempts equal to zero or less suspend the account and set attempts left to zero
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "500";
                                serviceRes.ReturnMsg = $"Account locked for user {objUser.MobileNumber} Please contact your administrator";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
            }
            return serviceRes;
        }

        public ServiceRes RegisterUser(Registration objRegister)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] parameter = new SqlParameter[19];
                parameter[0] = new SqlParameter { ParameterName = "@Mobile", Value = objRegister.MobileNumber };
                parameter[1] = new SqlParameter { ParameterName = "@Password", Value = _objRepo.Encrypt(objRegister.Password) };
                parameter[2] = new SqlParameter { ParameterName = "@EmailId", Value = objRegister.EmaillAddress };
                parameter[3] = new SqlParameter { ParameterName = "@Name", Value = objRegister.FullName };
                parameter[4] = new SqlParameter { ParameterName = "@BuildingName", Value = objRegister.Building_Name };
                parameter[5] = new SqlParameter { ParameterName = "@Locality", Value = objRegister.Locality };
                parameter[6] = new SqlParameter { ParameterName = "@Pincode", Value = objRegister.PinCode };
                parameter[7] = new SqlParameter { ParameterName = "@City", Value = objRegister.City };
                parameter[8] = new SqlParameter { ParameterName = "@State", Value = objRegister.State };
                parameter[9] = new SqlParameter { ParameterName = "@Landmark", Value = objRegister.Landmark };
                parameter[10] = new SqlParameter { ParameterName = "@AddressType", Value =(int)objRegister.AddressType };
                parameter[11] = new SqlParameter { ParameterName = "@CompanyName", Value = objRegister.CompanyName };
                parameter[12] = new SqlParameter { ParameterName = "@GSTNo", Value = objRegister.GST_No };
                parameter[13] = new SqlParameter { ParameterName = "@CategoryId", Value = objRegister.Category };
                parameter[14] = new SqlParameter { ParameterName = "@BusinessId", Value = objRegister.Businees_Type };
                parameter[15] = new SqlParameter { ParameterName = "@RoleId", Value = (int)objRegister.RoleId };
                parameter[16] = new SqlParameter { ParameterName = "@IpAddress", Value = objRegister.IpAddress };
                parameter[17] = new SqlParameter { ParameterName = "@PanNumber", Value = objRegister.PanNumber };
                parameter[18] = new SqlParameter { ParameterName = "@CompanyImage", Value = Convert.FromBase64String(objRegister.CompanyPhoto) };
                DataTable dt = SqlHelper.GetTableFromSP("Usp_RegisterUser", parameter);
                var returnValue = dt.Rows[0][0];
                if (Convert.ToInt32(returnValue) == 1 )
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "201";
                    serviceRes.ReturnMsg = "Username already exists.Please login with same username";
                }
                else if(Convert.ToInt32(returnValue) ==0)
                {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "User registered";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Error occured in procedure or database";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
            }
            return serviceRes;
        }

        public ServiceRes UnlockUserAccount(User objUser)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                LoginDetails loginDetails = new LoginDetails()
                {
                    MobileNumber = objUser.MobileNumber,
                    IsLocked = true,
                    LoginAttempts = 3
                };
                var returnValue = UpdateLoginDetails(loginDetails);
                if (returnValue == 1) {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Login details updated";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Login details not updated";
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
            }
            return serviceRes;
        }

        public ServiceRes UserProfile()
        {
            throw new NotImplementedException();
        }

        private int UpdateLoginDetails(LoginDetails loginDetails)
        {
            int returnValue=0;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = loginDetails.MobileNumber };
                param[1] = new SqlParameter { ParameterName = "@IsLocked", Value = loginDetails.IsLocked };
                param[2] = new SqlParameter { ParameterName = "@LoginAttempts", Value = loginDetails.LoginAttempts };
                param[3] = new SqlParameter { ParameterName = "@IsLogedIn", Value = loginDetails.IsLogedIn };
                returnValue = SqlHelper.ExecuteNonQuery("Usp_UpdateLoginDetails", param);
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
            }
            return returnValue;
        } 

    }
}
