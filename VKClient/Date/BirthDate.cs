
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Vkontakte.Date
{
    public class BirthDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public BirthDate(string date)
        {
            ParseDateString(date);
        }

        private void ParseDateString(string date)
        {
            string pattern = @"(?<day>[0-9]+)\.(?<month>[0-9]+)\.?(?<year>[0-9]+)?";
            var regExp = new Regex(pattern);
            var match = regExp.Match(date);
            int day;
            int.TryParse(match.Groups["day"].Value, out day);
            Day = day;

            int month;
            int.TryParse(match.Groups["month"].Value, out month);
            Month = month;

            int year;
            int.TryParse(match.Groups["year"].Value, out year);
            Year = year;

        }

        public override string ToString()
        {
            var day = Day.ToString();
            var month = Month.ToString();
            var year = Year.ToString();
            var sb = new StringBuilder("");

            if (day != "0")
            {
                sb.Append(day);    
            }
            
            if(month != "0")
            {
                sb.Append(".").Append(month);
            }

            if(year != "0")
            {
                sb.Append(".").Append(year);
            }

            return sb.ToString();
        }

        public DateTime ToDateTime()
        {
            throw new NotImplementedException();
        }
    }
}
