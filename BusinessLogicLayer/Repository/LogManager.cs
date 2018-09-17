using Repository;
using Entities;
using DataAccessLayer;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using static Enum.Enums;

namespace Repository
{
    public sealed class LogManager
    {
        private readonly string _logPath = String.Empty;
        public LogManager() {
            _logPath=Convert.ToString(ConfigurationManager.AppSettings["LogPath"]);
        }

        #region Write log to file
        //public static void WriteLog(Exception ex) {
        //    FileStream fileStream;
        //    FileInfo logFileInfo = new FileInfo(_logPath + "Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt");
        //    DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
        //    if (!logDirInfo.Exists) logDirInfo.Create();
        //    if (!logFileInfo.Exists)
        //    {
        //        fileStream = logFileInfo.Create();
        //    }
        //    else
        //    {
        //        fileStream = new FileStream(_logPath + "Log-" + DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt", FileMode.Append);
        //    }
        //    using (StreamWriter log = new StreamWriter(fileStream)) {
        //        log.WriteLine($"~~~~~~~ Stat Date {DateTime.Now} ~~~~~~~~~");
        //        log.WriteLine($"Error Source {ex.Source}");
        //        log.WriteLine($"Inner InnerException {ex.InnerException}");
        //        log.WriteLine($"Error Message {ex.Message}");
        //        log.WriteLine($"Error TargetSite {ex.TargetSite}");
        //        log.WriteLine($"~~~~~~~ End Date {DateTime.Now} ~~~~~~~~~");
        //    }
        //}
        #endregion

        #region Write log to database
        public static void WriteLog(Exception ex, SeverityLevel level)
        {
            
            try
            {
                var stackTrace = new StackTrace(ex, true);
                var frame = stackTrace.GetFrame(0);
                ErrorDetails objError = new ErrorDetails()
                {
                    Time = DateTime.Now,
                    SeverityLevel = level,
                    InnerInception = Convert.ToString(ex.InnerException),
                    MethodName = frame.GetMethod().Name,
                    Message = ex.Message,
                    Source =Convert.ToString(ex.Source),
                    LineNumber = frame.GetFileLineNumber()
                };

                SqlParameter[] parameter = new SqlParameter[7];
                parameter[0] = new SqlParameter { ParameterName = "@ErrorLevel", Value = (int)objError.SeverityLevel };
                parameter[1] = new SqlParameter { ParameterName = "@MethodName", Value = objError.MethodName };
                parameter[2] = new SqlParameter { ParameterName = "@LineNum", Value = objError.LineNumber };
                parameter[3] = new SqlParameter { ParameterName = "@Source", Value = objError.Source };
                parameter[4] = new SqlParameter { ParameterName = "@Message", Value = objError.Message };
                parameter[5] = new SqlParameter { ParameterName = "@ErrorDate", Value = objError.Time };
                parameter[6] = new SqlParameter { ParameterName = "@InnerInception", Value = objError.InnerInception };
                SqlHelper.ExecuteNonQuery("Usp_ErrorLog", parameter);
            }
            catch(Exception exce) {  }
        }
        #endregion
    }
}
