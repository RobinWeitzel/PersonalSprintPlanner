using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PersonalSprintPlanner.Helpers
{
    /*
        Blue = 1, // 0078D7
        Teal = 2, // 00B7C3
        Pink = 3, // E3008C
        Lila = 4, // 744DA9
        Green = 5, // 018574

    */
    class CustomColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case DataAccessLibrary.Models.Color.Transparent:
                    return new SolidColorBrush(Windows.UI.Colors.Transparent);
                case DataAccessLibrary.Models.Color.Blue:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 118, 215));
                case DataAccessLibrary.Models.Color.Teal:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 183, 195));
                case DataAccessLibrary.Models.Color.Pink:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 227, 0, 140));
                case DataAccessLibrary.Models.Color.Lila:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 116, 77, 169));
                case DataAccessLibrary.Models.Color.Green:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 1, 133, 116));
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
