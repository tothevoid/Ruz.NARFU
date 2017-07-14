using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RUZ.NARFU
{
    class TimeTable
    {
        public string GroupName { get; set; }
        public int GroupNum { get; set; }
        public List<Week> Weeks = new List<Week>();
    }

    class Week
    {
        public DateTime StartDate { get; set; }
        public List<Day> Days = new List<Day>();
    }

    class Day
    {
        public string Date { get; set; }
        public List<Pair> Pairs = new List<Pair>();
    }

    class Pair
    {
        public string Num { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string Place { get; set; }
        public string Name { get; set; }
        public string Lecturer { get; set; }
        public string Class { get; set; }
        public Thickness Margin { get; set; }
        public string Group { get; set; }
    }

    public class School
    {
        public City City { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
    }

    public class Faculty
    {
        public string Link { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Course { get; set; }
        //public StudyType Type { get; set; }
    }

}
