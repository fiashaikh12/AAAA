using System;
using static Enum.Enums;

namespace Repository
{
    public interface ILog
    {
        void WriteLog(Exception ex, SeverityLevel level);
    }
}
