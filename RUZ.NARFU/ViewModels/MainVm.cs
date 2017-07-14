using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace RUZ.NARFU
{
    class MainVm:VmBase
    {
        public MainVm()
        {
            Load();
        }

        public ICommand Settings { get { return new Command(LoadSettings); } }

        public ObservableCollection<Pair> Pairs { get; set; } = new ObservableCollection<Pair>();

        private void LoadSettings(object param)
        {
            string link = tableLink;
            var wnd = new TimeTableSelector();
            wnd.ShowDialog();
            if (link != tableLink)
            {
                Pairs.Clear();
                Load();  
            }
              

        }

        private void Load()
        {
            //XML data
            if (string.IsNullOrEmpty(tableLink))
            {
                var doc = XDocument.Load("Settings.xml");
                tableLink = @"http://ruz.narfu.ru/?timetable&group=" + doc.Element("Settings").Element("Table").Attribute("Link").Value;
            }

            var table = new TimeTableData();


            var data = table.GetTimeTable(tableLink);

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
                    Pairs.Add(pair);
                }

                foreach (var z in y.Pairs)
                {
                    z.Margin = new Thickness(200 * i + 20, 150 * j + 20, 0, 0);
                    j++;
                    Pairs.Add(z);
                }
                i++;
                j = 0;
            }
        }
    }
}
