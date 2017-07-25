using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUZ.NARFU
{
     class TimeTable
    {
        public string GroupName { get; set; }
        public string GroupNum { get; set; }
        public string LastChange { get; set; }
        public List<Week> Weeks = new List<Week>();
    }

}
