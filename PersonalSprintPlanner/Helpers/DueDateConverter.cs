using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace PersonalSprintPlanner.Helpers
{
    class DueDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return "not set";

            DateTime dt = DateTime.Parse(value.ToString());

            int dueIn = (int)Math.Floor((dt - DateTime.Now).TotalDays);

            if(dueIn < 1)
            {
                return Math.Abs(dueIn) + " days overdue";
            } else if (dueIn == -1)
            {
                return Math.Abs(dueIn) + " day overdue";
            } else if(dueIn == 0)
            {
                return "today";
            } else if(dueIn == 1)
            {
                return "1 day";
            } else
            {
                return dueIn + " days";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
