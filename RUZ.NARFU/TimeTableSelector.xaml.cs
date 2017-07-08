using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HtmlAgilityPack;
using System.IO;

namespace RUZ.NARFU
{
    /// <summary>
    /// Interaction logic for TimeTableSelector.xaml
    /// </summary>
    public partial class TimeTableSelector : Window
    {
        public TimeTableSelector()
        {
            InitializeComponent();
            var vm = new SelectorVm();
            DataContext = vm;
        }
    }

    class IndexToInstitute : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;

            if (TimeTableData.Schools.Count == 0)
                TimeTableData.GetUniversities();
            switch (index)
            {
                case 1:
                    return TimeTableData.GetNames(City.Arkhagelsk);
                case 2:
                    return TimeTableData.GetNames(City.Severodvinsk);
                default:
                    return TimeTableData.Schools.Select(x=>x.Name).ToList();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    class InstituteToFaculty : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string school = (string)value;

            TimeTableData.GetFaculties(school);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class TimeTableData
    {

        private static HtmlDocument doc = new HtmlDocument();

        public static List<School> Schools = new List<School>();
        
        public static List<string> GetNames(City city)
        {
            return  Schools.Where(x=>x.City == city).Select(x => x.Name).ToList();
        }

        public static void GetFaculties(string school)
        {
            string link = Schools.Where(x => x.Name == school).First().Link;

            LoadDoc(@"http://ruz.narfu.ru/"+link);
            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div");

        }
        public static void GetUniversities()
        {
            var link = @"http://ruz.narfu.ru";
            LoadDoc(link);
            var mainNode = doc.DocumentNode.SelectSingleNode("//body").SelectNodes("//div").Where(x=>x.Attributes.Count==2).Where(x => x.Attributes[1].Value== "classic").First();

            var divs = mainNode.ChildNodes.Where(x => x.Name == "div").Where(x => x.ChildNodes[1].Name == "a" && x.Attributes[0].Value.Contains("visible-xs"));


            foreach (var div in divs)
            {
                var elm = div.ChildNodes.Where(x => x.Name == "a").FirstOrDefault();
                if (elm == null)
                    continue;
                City city = City.Arkhagelsk;
                if (elm.InnerHtml.Contains("Северодвинск"))
                    city = City.Severodvinsk;
                Schools.Add(new School { City = city, Link = elm.Attributes[0].Value, Name = elm.InnerHtml.Trim() });

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

    public class School
    {
        public City City { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }
    }

    public enum City
    {
        Arkhagelsk,
        Severodvinsk
    }
}
