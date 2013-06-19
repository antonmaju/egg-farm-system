using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class ErrorInfo
    {
        public ErrorInfo()
        {
        }

        public ErrorInfo(string propertyName, string messageId)
        {
            PropertyName = propertyName;
            Message = messageId;
        }

        public string PropertyName { get; set; }

        public string Message { get; set; }
    }
}
