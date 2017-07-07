using System;
using System.Collections.Generic;
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
                    pair.Margin = new Thickness(50 * i + 20, 50 * j + 20, 0, 0);
                    j++;
                    pairs.Add(pair);
                }

                foreach (var z in y.Pairs)
                {
                    z.Margin = new Thickness(50 * i + 20, 50 * j + 20, 0, 0);
                    j++;
                    pairs.Add(z);
                }
                i++;
                j = 0;

            }

            ic.ItemsSource = pairs;
        }
    }
}
