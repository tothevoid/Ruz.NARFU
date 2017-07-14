using System;
using System.Collections.Generic;

namespace RUZ.NARFU
{
    public class Week
    {
        public DateTime StartDate { get; set; }
        public List<Day> Days = new List<Day>();
    }

}
