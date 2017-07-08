using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUZ.NARFU
{
    class SelectorVm:VmBase
    {
        public SelectorVm()
        {

        }

        public List<string> Cities { get; set; } = new List<string> {"Все", "Архангельск", "Северодвинск" };


    }
}
