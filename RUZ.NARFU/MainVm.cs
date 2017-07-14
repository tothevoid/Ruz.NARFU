using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Input;

namespace RUZ.NARFU
{
    class MainVm
    {
        public MainVm()
        {
            Load();
        }

        public ICommand Settings { get { return new Command(LoadSettings); } }

        public List<Pair> Pairs { get; set; } = new List<Pair>();

        private void LoadSettings()
        {
            var wnd = new TimeTableSelector();
            wnd.ShowDialog();
        }

        private void Load()
        {
            var doc = XDocument.Load("Settings.xml");

            TimeTableData.CurrentLink = @"http://ruz.narfu.ru/?timetable&group=" + doc.Element("Settings").Element("Table").Attribute("Link").Value;
            List<Pair> pairs = new List<Pair>();
            var data = LoadData.LoadAll();

            if (data == null)
                return;
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
                    pair.Margin = new Thickness(200 * i + 20, 150 * j + 20, 0, 0);
                    j++;
                    pairs.Add(pair);
                }

                foreach (var z in y.Pairs)
                {
                    z.Margin = new Thickness(200 * i + 20, 150 * j + 20, 0, 0);
                    j++;
                    pairs.Add(z);
                }
                i++;
                j = 0;
            }
            Pairs = pairs;
        }
    }
}
