using System.Windows;

namespace RUZ.NARFU
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainVm();
            DataContext = vm;
        }
    }
}

