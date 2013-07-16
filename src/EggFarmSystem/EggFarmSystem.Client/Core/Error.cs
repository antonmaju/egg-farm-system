using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core
{
    public class Error
    {
        public Error(){}

        public Error(Exception exception, object data)
        {
            Exception = exception;
            Data = data;
        }

        public Exception Exception { get; set; }

        public object Data { get; set; }
    }
}
