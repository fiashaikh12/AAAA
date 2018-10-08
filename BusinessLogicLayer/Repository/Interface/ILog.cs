using System;

namespace Repository
{
    public interface ILog
    {
        void WriteLog(Exception ex);
    }
}
