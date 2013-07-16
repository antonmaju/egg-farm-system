using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class ServiceException : Exception
    {
        public ServiceException()
        {
            
        }

        public ServiceException(string message) : base(message)
        {
            
        }
    }
}
