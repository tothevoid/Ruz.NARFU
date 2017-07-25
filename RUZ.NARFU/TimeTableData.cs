using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RUZ.NARFU
{
    public enum Result
    {
        Sucess,
        Failed
    }

    class TimeTableData
    {
        private  HtmlDocument doc = new HtmlDocument();

        public static List<School> Schools = new List<School>();

        public static List<Faculty> Faculties { get;} = new List<Faculty>();

        public Result LoadResult = Result.Failed;

        private HtmlDocument LoadPage(string path, HTMLType type = HTMLType.Web)
        {
            var doc = new HtmlDocument();

            if (path == null)
                throw new NullReferenceException();

            switch (type)
            {
                case (HTMLType.File):
                    {            
                        doc.Load(path, true);
                        return doc;
                    }
                case (HTMLType.Web):
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                        HttpWebResponse response;
                        try
                        {
                            response = (HttpWebResponse)request.GetResponse();
                        }
                        catch
                        {
                            MessageBox.Show("Соединение отсутствует");
                            return null;
                        }
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream receivedStream = response.GetResponseStream();
                            doc.Load(receivedStream, true);
                            response.Close();
                            LoadResult = Result.Sucess;
                            return doc;
                        }
                        break;
                    }
            }
            LoadResult = Result.Failed;
            return null;
            
      }

        private enum HTMLType
        {
            Web,
            File
        }


        //returns schools of choosen university
        public static List<string> GetNames(City city)
        {
            return Schools.Where(x => x.City == city).Select(x => x.Name).ToList();
        }

        //universities parser
        public void GetUniversities()
        {
            var link = @"http://ruz.narfu.ru";
            doc = LoadPage(link);

            if (doc == null)
                return;

            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div").Where(x => x.Attributes.Count == 2).Where(x => x.Attributes[1].Value == "classic").First();

            var divs = mainNode.ChildNodes.Where(x => x.Name == "div").Where(x => x.ChildNodes[1].Name == "a" && x.Attributes[0].Value.Contains("visible-xs"));

            foreach (var div in divs)
            {
                var cityTag = div.ChildNodes.Where(x => x.Name == "a").FirstOrDefault();
                if (cityTag == null)
                    continue;
                string name = cityTag.InnerHtml;
                City city = City.Arkhagelsk;
                if (cityTag.InnerHtml.Contains("Северодвинск"))
                {
                    city = City.Severodvinsk;
                    var sb = new StringBuilder();
                    var splitted = name.Split().Where(x => !string.IsNullOrEmpty(x)).TakeWhile(x => !x.Contains("nobr"));
                    foreach (var word in splitted)
                    {
                        sb.Append(word + " ");
                    }
                    name = sb.ToString();
                }
                else
                    name = name.Trim();
                Schools.Add(new School { City = city, Link = cityTag.Attributes[0].Value, Name = name });
            }
        }

        //faculties parser
        public void GetFaculties(string school)
        {
            //started with 1st course
            int currentCourse = 1;

            //shows which ones have parse troubles
            string type = "undefined";

            string link = Schools.Where(x => x.Name == school).First().Link;

            doc = LoadPage(@"http://ruz.narfu.ru/" + link);

            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div").Where(x => x.Attributes[0].Value == "tab-content").First();

            //all available courses
            var quantity = mainNode.ChildNodes.Where(x => x.Name == "div" && x.Attributes[1].Value.Contains("course")).Count();

            while (currentCourse <= quantity)
            {
                var selectedCourse = mainNode.ChildNodes.Where(x => x.Name == "div" && x.Attributes[1].Value == string.Format("course_{0}", currentCourse)).First();

                foreach (var child in selectedCourse.ChildNodes.Where(x => !x.Name.Contains("#")))
                {
                    if (child.Name == "h5")
                    {
                        type = child.InnerText;
                        continue;
                    }
                    var courseInfo = child.ChildNodes.Where(x => x.Name == "a").FirstOrDefault();
                    if (courseInfo == null)
                        continue;
                    var splited = courseInfo.InnerText.Split().Where(x => !string.IsNullOrEmpty(x)).ToList();
                    var sb = new StringBuilder();
                    foreach (var x in splited)
                    {
                        sb.Append($"{x} ");
                    }
                    Faculties.Add(new Faculty { Type = type, Name = sb.ToString(), Link = courseInfo.Attributes[1].Value, Course = currentCourse });
                }
                currentCourse++;
            }
        }

        public TimeTable GetTimeTable(string link)
        {
            //load via webrequest
            HtmlDocument doc;
           
           // doc = LoadPage(link);
           
          
            //load via file
         
            doc = LoadPage("ruz2.html", HTMLType.File);
           
               

            if (doc == null)
                return null;

            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div");

            var timeTable = new TimeTable();

            var tabContent = mainNode.Where(x => x.Attributes[0].Value == "container-fluid content" && x.Attributes[0].Name == "class").FirstOrDefault();

            if (tabContent == null)
                return null;

            //Date of last table's update
            
            string lastChange = tabContent.ChildNodes[1].InnerHtml;

            timeTable.LastChange = WebUtility.HtmlDecode(lastChange).Trim(); ;

            var table = tabContent.ChildNodes.Where(x => x.Name == "div").FirstOrDefault();

            if (table == null)
                return null;

            string[] groupInfo = table.ChildNodes.Where(x => x.Name == "h4").FirstOrDefault().InnerHtml.Split().Where(x => !string.IsNullOrEmpty(x)).ToArray();

            timeTable.GroupNum = groupInfo[0];

            var sb = new StringBuilder();
            foreach (var x in groupInfo.Skip(1))
            {
                sb.Append($"{x} ");
            }

            timeTable.GroupName = sb.ToString();

            var weeks = table.ChildNodes.Where(x => x.Name == "div").ToList();

            if (weeks.Count != 6)
                return null;

            foreach (var week in weeks)
            {
                var currentWeek = new Week();
                var days = week.ChildNodes.Where(x => x.Name == "div").ToList();

                foreach (var day in days)
                {
                    var info = day.ChildNodes.Where(x => x.Name == "div").First().InnerText.Split().Where(x => !string.IsNullOrEmpty(x)).ToList();
                    Day currentDay = new Day { Date = info[1] };
                    foreach (var pairs in day.ChildNodes.Where(x => x.Name == "div" && x.Attributes[0].Value.Contains("hidden-xs") && !x.Attributes[0].Value.Contains("dayofweek")))
                    {
                        var spans = pairs.ChildNodes.Where(x => x.Name == "span").ToList();
                        var Pair = new Pair();
                        if (spans.Count() == 1)
                            Pair.Num = spans[0].InnerText;
                        else
                        {
                            foreach (var pair in spans)
                            {
                                switch (pair.Attributes[0].Value)
                                {
                                    case ("time_para"):
                                        Pair.Time = WebUtility.HtmlDecode(pair.InnerText).Trim();
                                        break;
                                    case ("num_para"):
                                        Pair.Num = pair.InnerText;
                                        break;
                                    case ("kindOfWork"):
                                        Pair.Type = pair.InnerText;
                                        break;
                                    case ("discipline"):
                                        Pair.Name = pair.InnerText;
                                        if (pair.ChildNodes.Where(x => x.Name == "nobr").FirstOrDefault() != null)
                                            Pair.Lecturer = pair.ChildNodes.Where(x => x.Name == "nobr").First().InnerText;
                                        break;
                                    case ("auditorium"):
                                        var res = pair.ChildNodes.Where(x => x.Name == "#text").LastOrDefault();
                                        Pair.Place = res?.InnerHtml.Replace(',', ' ').Trim();
                                        Pair.Class = WebUtility.HtmlDecode(pair.ChildNodes.Where(x => x.Name == "b").First().InnerText);
                                        break;
                                    case ("lecturer"):
                                        Pair.Lecturer = pair.InnerText;
                                        break;
                                    case ("group"):
                                        Pair.Group = pair.InnerText;
                                        break;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(Pair.Name))
                            Pair.Num = $"{Pair.Num} ({Pair.Time})";
                        if (!string.IsNullOrEmpty(Pair.Lecturer))
                        {
                            if (Pair.Name.Contains(Pair.Lecturer))
                            {
                                Pair.Lecturer = null;
                            }
                        }
                        currentDay.Pairs.Add(Pair);
                    }
                    currentWeek.Days.Add(currentDay);
                }
                timeTable.Weeks.Add(currentWeek);
            }
            return timeTable;
        }
    }
}
