using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUZ.NARFU
{
    public class TimeTable
    {
        public string GroupName { get; set; }
        public int GroupNum { get; set; }
        public List<Week> Weeks = new List<Week>();
    }

}
