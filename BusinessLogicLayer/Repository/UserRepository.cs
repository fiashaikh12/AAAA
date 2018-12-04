using System;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Data;
using Entities;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptographyRepository _objRepo = CryptographyRepository.GetInstance;
        private readonly EmailerRepository _emailerRepository = EmailerRepository.GetInstance;
        ICommonRepository commonRepository = new CommonRepository();
        public UserRepository() { }

        public ServiceRes ChangePassword(ChangePassword changePassword)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                if (!string.IsNullOrEmpty(changePassword.Username))
                {
                    SqlParameter[] sqlParameters = new SqlParameter[1];
                    sqlParameters[0] = new SqlParameter { ParameterName = "@mobileNumber", Value = changePassword.Username };

                    var oldPassword = SqlHelper.GetTableFromSP("Usp_GetUserPassword", sqlParameters);
                    if (_objRepo.Decrypt(Convert.ToString(oldPassword.Rows[0][0])).Equals(changePassword.OldPassword))
                    {
                        SqlParameter[] sqlParameter = new SqlParameter[2];
                        sqlParameter[0] = new SqlParameter { ParameterName = "@mobileNumber", Value = changePassword.Username };
                        sqlParameter[1] = new SqlParameter { ParameterName = "@newPassword", Value = changePassword.NewPassword };

                        var passwordChanged = SqlHelper.ExecuteNonQuery("Usp_UpdatePassword", sqlParameter);
                        if (passwordChanged == 1)
                        {
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
                    else
                    {
                        serviceRes.IsSuccess = false;
                        serviceRes.ReturnCode = "202";
                        serviceRes.ReturnMsg = "Old password is not correct";
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes ForgetPassword(User user)
        {
            ServiceRes serviceRes = new ServiceRes();
            try {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter { ParameterName = "@mobileNumber", Value = user.MobileNumber };
                DataTable dataTable = SqlHelper.GetTableFromSP("Usp_GetUserDetails", parameter);
                if (dataTable.Rows.Count > 0)
                {
                    string emailAddress = Convert.ToString(dataTable.Rows[0][0]);
                    string password = this._objRepo.Decrypt(Convert.ToString(dataTable.Rows[0][1]));
                    if (!string.IsNullOrEmpty(emailAddress))
                    {
                        if (this._emailerRepository.Send(emailAddress, password))
                        {
                            serviceRes.IsSuccess = true;
                            serviceRes.ReturnCode = "200";
                            serviceRes.ReturnMsg = "Password reset link has been sent to your registered email address.";
                        }
                        else
                        {
                            serviceRes.IsSuccess = false;
                            serviceRes.ReturnCode = "400";
                            serviceRes.ReturnMsg = "Something went wrong";
                        }
                    }
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "404";
                    serviceRes.ReturnMsg = "Data not found";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes IsUserValid(User objUser)
        {
            ServiceRes<LoginResponse> serviceRes = new ServiceRes<LoginResponse>();
            try
            {
                LoginDetails loginDetails = new LoginDetails() ;
                //TokenRepository tokenRepository = TokenRepository.GetInstance;
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
                        int memberId= Convert.ToInt32(dtUser.Rows[0][0]);
                        string decryptedPassword = _objRepo.Decrypt(Convert.ToString(dtUser.Rows[0][2]));
                        string username = Convert.ToString(dtUser.Rows[0][1]);
                        bool isLocked = Convert.ToBoolean(dtUser.Rows[0][3]);
                        int loginAttempts = Convert.ToInt32(dtUser.Rows[0][4]);
                        string companyName = Convert.ToString(dtUser.Rows[0][7]); ;
                        string fullName = Convert.ToString(dtUser.Rows[0][6]); ;
                        //bool isLogedIn = Convert.ToBoolean(dtUser.Rows[0][5]);
                        int roleId = Convert.ToInt32(dtUser.Rows[0][5]);
                        
                        if ((username.Equals(objUser.MobileNumber) && decryptedPassword.Equals(objUser.Password))) {
                            if (!isLocked) {
                                //reset login attempts and is locked column
                                LoginResponse loginResponse = new LoginResponse
                                {
                                    CompanyName = companyName,
                                    FullName = fullName,
                                    RoleId = roleId,
                                    UserId = memberId
                                };
                                loginDetails.MobileNumber = objUser.MobileNumber;
                                loginDetails.IsLocked = false;
                                loginDetails.LoginAttempts = 3;
                                loginDetails.IsLogedIn = true;
                                //UpdateLoginDetails(loginDetails);
                                
                                serviceRes.IsSuccess = true;
                                serviceRes.ReturnCode = "200";
                                serviceRes.ReturnMsg = $"User authenticated";
                                serviceRes.Data = loginResponse;//tokenRepository.GenerateToken(memberId);
                            }
                            else {
                                //return account suspended status
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "403";
                                serviceRes.ReturnMsg = $"Account suspended for user {objUser.MobileNumber}. Please contact your administrator";
                            }
                        }
                        else if (String.IsNullOrEmpty(objUser.Password) || decryptedPassword != objUser.Password)
                        {
                            //decrese login attempts
                            //if attempts greater than zero set account status active and decrease attempts
                            loginAttempts = loginAttempts - 1;
                            if (loginAttempts > 0)
                            {
                                loginDetails.MobileNumber = objUser.MobileNumber;
                                loginDetails.IsLocked = false;
                                loginDetails.LoginAttempts = loginAttempts;
                                loginDetails.IsLogedIn = false;
                                //UpdateLoginDetails(loginDetails);
                                //update attempts left and return status as wrong password
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "403";
                                serviceRes.ReturnMsg = $"Wrong password attempts left {loginAttempts} ";
                            }
                            else
                            {
                                //update if attempts equal to zero or less suspend the account and set attempts left to zero
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "500";
                                serviceRes.ReturnMsg = $"Account locked for user {objUser.MobileNumber}. Please contact your administrator";
                            }
                        }
                        UpdateLoginDetails(loginDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes RegisterUser(Registration objRegister)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                //string tempLocation = commonRepository.Base64toImage(objRegister.CompanyPhoto, "TempImages", "Temp", "Photos");
                string fileLocation = commonRepository.Base64toImage(objRegister.CompanyPhoto, "Images", "CompanyPhoto","CompanyPhoto");
                SqlParameter[] parameter = new SqlParameter[20];
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
                parameter[13] = new SqlParameter { ParameterName = "@BusinessId", Value = objRegister.BusinessId };
                parameter[14] = new SqlParameter { ParameterName = "@RoleId", Value = (int)objRegister.RoleId };
                parameter[15] = new SqlParameter { ParameterName = "@IpAddress", Value = objRegister.IpAddress };
                parameter[16] = new SqlParameter { ParameterName = "@PanNumber", Value = objRegister.PanNumber };
                parameter[17] = new SqlParameter { ParameterName = "@CompanyImage", Value = fileLocation };
                parameter[18] = new SqlParameter { ParameterName = "@Latitude", Value = objRegister.Latitude };
                parameter[19] = new SqlParameter { ParameterName = "@Longitude", Value = objRegister.Longitude};
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
                    //commonRepository.RemoveFileFromDirectory("TempImages", "Temp", tempLocation);
                    //commonRepository.Base64toImage(objRegister.CompanyPhoto, "Images", "CompanyPhoto", "CompanyPhoto");
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
                LogManager.WriteLog(ex);
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
                LogManager.WriteLog(ex);
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
                LogManager.WriteLog(ex);
            }
            return returnValue;
        } 

    }
}
