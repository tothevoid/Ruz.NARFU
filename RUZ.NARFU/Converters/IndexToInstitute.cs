using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace RUZ.NARFU
{
    class IndexToInstitute : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;

            if (TimeTableData.Schools.Count == 0)
                TimeTableData.GetUniversities();
            switch (index)
            {
                case 1:
                    return TimeTableData.GetNames(City.Arkhagelsk);
                case 2:
                    return TimeTableData.GetNames(City.Severodvinsk);
                default:
                    return TimeTableData.Schools.Select(x=>x.Name).ToList();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    
   
}
