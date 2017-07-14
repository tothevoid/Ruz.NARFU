using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RUZ.NARFU
{
    class InstituteToFaculty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string school = (string)value;

            if (TimeTableData.Faculties.Count != 0)
                TimeTableData.Faculties.Clear();

            var table = new TimeTableData();
            table.GetFaculties(school);

            return TimeTableData.Faculties.Select(x=>x.Name).ToList();
          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    } 
}
