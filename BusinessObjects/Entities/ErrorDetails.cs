using System;
using static Enum.Enums;

namespace Entities
{
    public class ErrorDetails
    {
        public DateTime ? Time { get; set; }
        public SeverityLevel SeverityLevel { get; set; }
        public string  InnerInception { get; set; }
        public string MethodName { get; set; }
        public string  Source { get; set; }
        public string  Message { get; set; }
        public int LineNumber { get; set; }
    }
}
