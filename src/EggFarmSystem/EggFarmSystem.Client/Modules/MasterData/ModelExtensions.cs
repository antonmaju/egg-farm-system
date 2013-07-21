using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.MasterData
{
    public static class ModelExtensions
    {
        public static string ToDescription(this ConsumableType type)
        {
            var attributes = (DescriptionAttribute[])type.GetType().GetField(type.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            string description = attributes.Length > 0 ? attributes[0].Description : string.Empty;
            string result = string.Empty;
            if (!string.IsNullOrEmpty(description))
            {
                result = typeof(LanguageData).GetProperty(description).GetValue(null, null) as string;
            }
            return result;
        }
    }
}
