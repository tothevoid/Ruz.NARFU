using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace RUZ.NARFU
{
    class MainVm:VmBase
    {
        public MainVm()
        {
            Load();
        }

        public ICommand Settings { get { return new Command(LoadSettings); } }

        //public ObservableCollection<Pair> Pairs { get; set; } = new ObservableCollection<Pair>();

        private string tableInfo;

        public string TableInfo { get { return tableInfo; } set { Set(ref tableInfo, value); } }

        private Pair[][] pairs;

        public Pair[][] Pairs { get { return pairs; } set { if (value != pairs) pairs = value; OnPropertyChanged("Pairs");  } }

        public Array Days { get; set; }

        private void LoadSettings(object param)
        {           
            string link = tableLink;
            var wnd = new TimeTableSelector();
            wnd.ShowDialog();
            if (link != tableLink)
            {
                Load();  
            }
        }

        //private void Load()
        //{
        //    //XML data


        //    if (string.IsNullOrEmpty(tableLink))
        //    {
        //        var doc = XDocument.Load("Settings.xml");
        //        tableLink = @"http://ruz.narfu.ru/?timetable&group=" + doc.Element("Settings").Element("Table").Attribute("Link").Value;
        //    }

        //    var table = new TimeTableData();


        //    var data = table.GetTimeTable(tableLink);

        //    if (data == null)
        //        return;
        //    int i = 0, j = 0;

        //    var x = data.Weeks[0];
            

        //    foreach (var y in x.Days)
        //    {
        //        if (y.Pairs.Count == 0)
        //        {
        //            i++;
        //            continue;
        //        }
        //        if (y.Pairs[0].Num != "1")
        //        {
        //            var pair = new Pair();
        //            pair.Margin = new Thickness(200 * i + 20, 150 * j + 20, 0, 0);
        //            j++;
        //            Pairs.Add(pair);
        //        }

        //        foreach (var z in y.Pairs)
        //        {
        //            z.Margin = new Thickness(200 * i + 20, 150 * j + 20, 0, 0);
        //            j++;
        //            Pairs.Add(z);
        //        }
        //        i++;
        //        j = 0;
        //    }
        //}

        private void Load()
        {
            if (string.IsNullOrEmpty(tableLink))
            {
                var doc = XDocument.Load("Settings.xml");
                tableLink = @"http://ruz.narfu.ru/?timetable&group=" + doc.Element("Settings").Element("Table").Attribute("Link").Value;
            }
            var days = Enumerable.Range(0, 1).Select(day => new Headers[6]).ToArray();

            var table = new TimeTableData();
            var data = table.GetTimeTable(tableLink);

            TableInfo = $"{data.GroupNum} - {data.GroupName} ({data.LastChange})";

            if (data == null)
                return;

            var x = data.Weeks[0];

            int maxcount = 0;
            foreach (var y in x.Days)
            {
                if (y.Pairs.Count() > maxcount)
                    maxcount = y.Pairs.Count;
            }

            var tableArray = Enumerable.Range(0, maxcount).Select(pair => new Pair[6]).ToArray();

            int currDay = 0;
            int currPair = 0;
            foreach (var y in x.Days)
            {
                var day = DateTime.Parse(y.Date);

                days[0][currDay] = new Headers { DayInfo = $"{day.ToString("d")}, {day.DayOfWeek.ToString()}"};

                foreach (var z in y.Pairs)
                {
                    tableArray[currPair][currDay] = z;
                    currPair++;
                }
                currPair = 0;
                currDay++;
            }
            Pairs = tableArray;
            Days = days;
        }
    }
    
}
