using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using ServiceStack.Text;

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
            if (error == null || error.Exception == null || string.IsNullOrEmpty(error.Exception.Message))
                return null;

            string msg = null;
            if (error.Exception.Message.StartsWith("["))
            {
                var serializer = new JsonSerializer<List<ErrorInfo>>();
                try
                {
                    var errorList = serializer.DeserializeFromString(error.Exception.Message);
                    var sb = new StringBuilder();
                    foreach (var errorInfo in errorList)
                    {
                        sb.AppendLine(LanguageHelper.GetValue(errorInfo.Message));
                    }
                    msg = sb.ToString();
                }
                catch
                {
                    
                }
            }
            else
            {
                string errMessage = error.Exception.Message;
                if (errMessage.StartsWith("\""))
                    errMessage = errMessage.Replace("\"", "");

                msg = LanguageHelper.GetValue(errMessage);
               
            }

             if (string.IsNullOrEmpty(msg))
                    msg = error.Exception.Message;

            return msg;
        }
    }
}
