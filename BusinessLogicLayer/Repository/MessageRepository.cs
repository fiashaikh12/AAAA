using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DataAccessLayer;
using Entities;
using Interface;
namespace Repository
{
    public class MessageRepository : IMessage
    {
        public ServiceRes AllMessages(Messages messages)
        {
            ServiceRes<List<Messages>> serviceRes = new ServiceRes<List<Messages>>();
            try
            {
                SqlParameter[] sqlParameter = new SqlParameter[4];
                sqlParameter[0] = new SqlParameter { ParameterName = "@senderId", Value = messages.SenderId };
                sqlParameter[3] = new SqlParameter { ParameterName = "@flag", Value = "G" };
                var dataTable = SqlHelper.GetTableFromSP("Usp_MessageMaster", sqlParameter);
                if (dataTable!=null || dataTable.Rows.Count >0)
                {
                    serviceRes.Data = dataTable.AsEnumerable().Select(x => new Messages {
                        MessageContent=x.Field<string>("Msg_Description"),
                        RecieverId=x.Field<int>("Recipent_Id"),
                        SenderId=x.Field<int>("Sender_Id"),
                        IsRead=x.Field<bool>("Read_Flag")
                    }).ToList();
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
            catch (Exception ex)
            {
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Exception occured";
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }

        public ServiceRes Send(Messages messages)
        {
            ServiceRes serviceRes = new ServiceRes();
            try {
                SqlParameter[] sqlParameter = new SqlParameter[4];
                sqlParameter[0] = new SqlParameter { ParameterName = "@senderId", Value = messages.SenderId };
                sqlParameter[1] = new SqlParameter { ParameterName = "@reciId", Value = messages.RecieverId };
                sqlParameter[2] = new SqlParameter { ParameterName = "@msgDiscription", Value = messages.MessageContent };
                sqlParameter[3] = new SqlParameter { ParameterName = "@flag", Value ="S" };
                int returnValue = SqlHelper.ExecuteNonQuery("Usp_MessageMaster", sqlParameter);
                if(returnValue > 0)
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
                serviceRes.IsSuccess = false;
                serviceRes.ReturnCode = "500";
                serviceRes.ReturnMsg = "Exception occured";
                LogManager.WriteLog(ex);
            }
            return serviceRes;
        }
    }
}
