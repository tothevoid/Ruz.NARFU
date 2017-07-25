using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RUZ.NARFU
{
    public class SelectorVm:VmBase
    {
        public SelectorVm()
        {
            //TODO: dynamic change
            LastUpdate = $"Обновлено: {DateTime.Now.ToString()}";
        }

        //TODO: link callback

        public event Action Loaded;

        public List<string> Cities { get; } = new List<string> {"Все", "Архангельск", "Северодвинск" };

        private int cityIndex;

        public int CityIndex { get { return cityIndex; } set {Set(ref cityIndex,value); } }

        private bool selection;

        public bool Selection { get { return selection; } set { Set(ref selection, value); } }

        private string lastUpdate;

        public string LastUpdate
        {
            get { return lastUpdate; }
            set { Set(ref lastUpdate, value); OnPropertyChanged("LastUpdate"); }
        }

        public ICommand OkBtn { get { return new Command(Load); } }
       
        private void Load(object param)
        {
            if (param==null)
            {
                MessageBox.Show("Выберите факультет");
                return;
            }
            string name = (string)param;
            tableLink = @"http://ruz.narfu.ru/" + TimeTableData.Faculties.Where(x => x.Name == name).First().Link;
            if (Selection)
                Settings.Group = tableLink;
            Loaded.Invoke();


        }

    }
}
