using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EggFarmSystem.Client.Controls
{
    public class ToggleButtonHelper : DependencyObject
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached(
            "Icon", typeof (ImageSource), typeof (ToggleButtonHelper));

        public static void SetIcon(ToggleButton button, ImageSource image)
        {
            button.SetValue(IconProperty, image);
        }

        public static ImageSource GetIcon(ToggleButton button)
        {
            return button.GetValue(IconProperty) as ImageSource;
        }
    }
}
