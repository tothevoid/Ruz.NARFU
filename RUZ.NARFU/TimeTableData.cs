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

        public static string CurrentLink { get; set; }

        private static HtmlDocument doc = new HtmlDocument();

        public static List<School> Schools = new List<School>();

        public static List<Faculty> Faculties { get; private set; } = new List<Faculty>();
        public static List<string> GetNames(City city)
        {
            return Schools.Where(x => x.City == city).Select(x => x.Name).ToList();
        }

        public static void GetFaculties(string school)
        {
            // TODO: change to input args



            int currentCourse = 1;

            //  StudyType type = StudyType.First;
            string type = "undefined";

            string link = Schools.Where(x => x.Name == school).First().Link;


            LoadDoc(@"http://ruz.narfu.ru/" + link);
            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div").Where(x => x.Attributes[0].Value == "tab-content").First();

            //all available courses
            var quantity = mainNode.ChildNodes.Where(x => x.Name == "div" && x.Attributes[1].Value.Contains("course")).Count();

            while (currentCourse <= quantity)
            {
                //null detect
                var selectedCourse = mainNode.ChildNodes.Where(x => x.Name == "div" && x.Attributes[1].Value == string.Format("course_{0}", currentCourse)).First();

                foreach (var child in selectedCourse.ChildNodes.Where(x => !x.Name.Contains("#")))
                {
                    if (child.Name == "h5")
                    {
                        type = child.InnerText;
                        //type = StudyType.Second;
                        continue;
                    }

                    //null detect
                    var e = child.ChildNodes.Where(x => x.Name == "a").FirstOrDefault();
                    if (e == null)
                        continue;
                    var splited = e.InnerText.Split().Where(x => !string.IsNullOrEmpty(x)).ToList();
                    var sb = new StringBuilder();
                    foreach (var x in splited)
                    {
                        sb.Append(x + " ");
                    }
                    Faculties.Add(new Faculty { Type = type, Name = sb.ToString(), Link = e.Attributes[1].Value, Course = currentCourse });

                }
                currentCourse++;
            }

        }
        public static void GetUniversities()
        {
            var link = @"http://ruz.narfu.ru";
            LoadDoc(link);
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


        private static void LoadDoc(string path)
        {
            //no connection exception
            doc = new HtmlDocument();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();

                doc.Load(receiveStream, true);
                response.Close();
            }

        }
    }

    public enum City
    {
        Arkhagelsk,
        Severodvinsk
    }

}
