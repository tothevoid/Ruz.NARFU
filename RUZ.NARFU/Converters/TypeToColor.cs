using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RUZ.NARFU
{
    public class TypeToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (string)value;
            switch (type)
            {
                case ("Консультация"):
                    return new SolidColorBrush(Colors.Brown);
                case ("Экзамен"):
                    return new SolidColorBrush(Colors.Pink);
                case ("Практические занятия"):
                    return new SolidColorBrush(Colors.LightYellow);
                case ("Лабораторные занятия"):
                    return new SolidColorBrush(Colors.LightBlue);
                case ("Лекции"):
                    return new SolidColorBrush(Colors.LightGreen);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
