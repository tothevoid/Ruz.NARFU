using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RUZ.NARFU
{
    public class TimeTableData
    {
        private  HtmlDocument doc = new HtmlDocument();

        public static List<School> Schools = new List<School>();

        public static List<Faculty> Faculties { get; private set; } = new List<Faculty>();

        private HtmlDocument LoadPage(string path)
        {
            if (path == null)
                throw new NullReferenceException();
            doc = new HtmlDocument();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                doc.Load(receiveStream, true);
                response.Close();
                return doc;
            }
            return null;
        }

        public static List<string> GetNames(City city)
        {
            return Schools.Where(x => x.City == city).Select(x => x.Name).ToList();
        }

        public void GetUniversities()
        {
            var link = @"http://ruz.narfu.ru";
            doc = LoadPage(link);

            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div").Where(x => x.Attributes.Count == 2).Where(x => x.Attributes[1].Value == "classic").First();

            var divs = mainNode.ChildNodes.Where(x => x.Name == "div").Where(x => x.ChildNodes[1].Name == "a" && x.Attributes[0].Value.Contains("visible-xs"));

            foreach (var div in divs)
            {
                var elm = div.ChildNodes.Where(x => x.Name == "a").FirstOrDefault();
                if (elm == null)
                    continue;
                string name = elm.InnerHtml;
                City city = City.Arkhagelsk;
                if (elm.InnerHtml.Contains("Северодвинск"))
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
                Schools.Add(new School { City = city, Link = elm.Attributes[0].Value, Name = name });

            }
        }

        public void GetFaculties(string school)
        {
            //started with 1 course
            int currentCourse = 1;

            //shows which ones have parse troubles
            string type = "undefined";

            string link = Schools.Where(x => x.Name == school).First().Link;

            LoadPage(@"http://ruz.narfu.ru/" + link);

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
                        sb.Append(x + " ");
                    }
                    Faculties.Add(new Faculty { Type = type, Name = sb.ToString(), Link = courseInfo.Attributes[1].Value, Course = currentCourse });
                }
                currentCourse++;
            }
        }

        public TimeTable GetTimeTable(string link)
        {
           //var doc = LoadPage(link);

            var doc = new HtmlDocument();
            doc.Load("ruz2.html", true);

            if (doc == null)
                return null;

            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div");

            var timeTable = new TimeTable();
            //TODO: set group's name and num

            var tabContent = mainNode.Where(x => x.Attributes[0].Value == "container-fluid content" && x.Attributes[0].Name == "class").FirstOrDefault();

            if (tabContent == null)
                return null;

            //Date of last table's update
            string lastChange = tabContent.ChildNodes[1].InnerHtml;

            var table = tabContent.ChildNodes.Where(x => x.Name == "div").FirstOrDefault();

            if (table == null)
                return null;

            var weeks = table.ChildNodes.Where(x => x.Name == "div").ToList();

            if (weeks.Count != 6)
                return null;

            foreach (var week in weeks)
            {
                var currentWeek = new Week();
                // TODO: set timeline of current week
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
