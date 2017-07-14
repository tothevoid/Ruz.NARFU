using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Windows;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace RUZ.NARFU
{
    class LoadData
    {
        public static TimeTable LoadAll()
        {
            var doc = GetPage(TimeTableData.CurrentLink);

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

        public static HtmlDocument GetPage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

          //  path = "http://ruz.narfu.ru/" + path;
            HtmlDocument doc = new HtmlDocument();
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
    }


}
