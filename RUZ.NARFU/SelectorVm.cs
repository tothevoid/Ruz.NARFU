using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RUZ.NARFU
{
    class SelectorVm:VmBase
    {
        public SelectorVm()
        {

        }

        //TODO: link callback

        public event Action Loaded;

        public List<string> Cities { get; set; } = new List<string> {"Все", "Архангельск", "Северодвинск" };

        public ICommand OkBtn { get { return new Command(Load); } }
       
        private void Load(object param)
        {
            string name = (string)param;
            TimeTableData.CurrentLink  =  TimeTableData.Faculties.Where(x => x.Name == name).First().Link;
            Loaded.Invoke();
        }


    }
}
