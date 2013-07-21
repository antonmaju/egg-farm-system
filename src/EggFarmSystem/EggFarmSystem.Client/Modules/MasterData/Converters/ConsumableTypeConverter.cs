using System;
using System.Windows.Data;
using EggFarmSystem.Models;

namespace EggFarmSystem.Client.Modules.MasterData.Converters
{
    [ValueConversion(typeof(ConsumableType),typeof(string))]
    public class ConsumableTypeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int number = System.Convert.ToInt32(value);
            var consumableType = (ConsumableType) number;
            return consumableType.ToDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
