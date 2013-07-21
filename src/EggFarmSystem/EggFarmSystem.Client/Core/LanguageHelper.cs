using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Core
{
    public static class LanguageHelper
    {
        public static string GetValue(string resourceId)
        {
            try
            {
                return typeof(LanguageData).GetProperty(resourceId).GetValue(null, null) as string;
            }
            catch
            {
                return null;
            }
        }

        public static string TryGetErrorMessage(object param)
        {
            var error = param as Error;
            if (error == null)
                return null;

            string msg = LanguageHelper.GetValue(error.Exception.Message);
            if (string.IsNullOrEmpty(msg))
                msg = error.Exception.Message;

            return msg;
        }
    }
}
