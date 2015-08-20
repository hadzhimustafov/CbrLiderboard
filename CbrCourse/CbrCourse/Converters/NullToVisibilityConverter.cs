using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace CbrCourse.Converters
{
    class NullToVisibilityConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string direction = parameter as string;
            if (!string.IsNullOrEmpty(direction) && string.Equals(direction,"invert", StringComparison.CurrentCultureIgnoreCase))
            {
                return value == null ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return value == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
