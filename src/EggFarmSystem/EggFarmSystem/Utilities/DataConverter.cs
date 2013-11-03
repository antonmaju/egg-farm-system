using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Utilities
{
    public static class DataConverter
    {
        public static int ToInt32(object o)
        {
            return o != null && o != DBNull.Value ? Convert.ToInt32(o) : 0;
        }
        
        public static decimal ToDecimal(object o)
        {
            return o != null && o != DBNull.Value ? Convert.ToDecimal(o) : 0;
        }

        public static Guid ToGuid(object o)
        {
            return o != null && o != DBNull.Value ? new Guid(o.ToString()) : Guid.Empty;
        }

        public static string ToString(object o)
        {
            return o != null && o != DBNull.Value ? o.ToString() : null;
        }
    }
}
