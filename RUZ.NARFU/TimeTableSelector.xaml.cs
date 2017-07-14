using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.IO;

namespace RUZ.NARFU
{

    public partial class TimeTableSelector : Window
    {
        public TimeTableSelector()
        {
            InitializeComponent();
            var vm = new SelectorVm();
            vm.Loaded += Vm_Loaded;
            DataContext = vm;
        }

        private void Vm_Loaded()
        {
            this.Close();
        }
    }
}
