using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RUZ.NARFU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            List<Pair> pairs = new List<Pair>();
            var data = LoadData.LoadAll();
            int i = 0, j = 0;

            var x = data.Weeks[0];


            foreach (var y in x.Days)
            {
                if (y.Pairs.Count == 0)
                {
                    i++;
                    continue;
                }
                if (y.Pairs[0].Num != "1")
                {
                    var pair = new Pair();
                    pair.Margin = new Thickness(230 * i + 20, 150 * j + 20, 0, 0);
                    j++;
                    pairs.Add(pair);
                }

                foreach (var z in y.Pairs)
                {
                    z.Margin = new Thickness(230 * i + 20, 150 * j + 20, 0, 0);
                    j++;
                    pairs.Add(z);
                }
                i++;
                j = 0;
            }
            ic.ItemsSource = pairs;
        }
    }

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
                    return new SolidColorBrush( Colors.Pink);
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

