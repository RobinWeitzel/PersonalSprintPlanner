using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PersonalSprintPlanner.Helpers
{
    public class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch(value)
            {
                case Priority.High:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 232, 17, 35));
                case Priority.Medium:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 187, 0));
                case Priority.Low:
                    return new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 204, 105));
                default: 
                    return new SolidColorBrush(Colors.Gray);
            } 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
