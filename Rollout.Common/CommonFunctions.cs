using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace Rollout.Common
{
    public static class CommonFunctions
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Convert a date string like "4/2/2020" into a JDE Julian Date
        /// Which has the format of CYYDDD where 
        /// C = Century (0 = 19--, 1 = 20--, 2 = 21--, etc.)
        /// YY = The last 2 digits of the year
        /// DDD = The day of the year
        /// </summary>
        /// <param name="USADateFormat">MM/DD/YYYY format</param>
        /// <returns>A JDE Julian Date</returns>
        public static uint DateStringToJulian(string USADateFormat)
        {
            DateTime ParsedDate = DateTime.Parse(USADateFormat);
            int century = ParsedDate.Year;
            century = (int)Math.Truncate(((double)century / 100)) - 19; // 0 = 19--, 1 = 20--, 2 = 21--, etc.
            uint JulianDate = (uint)century * 100000; // format is CYYDDD
            int year = ParsedDate.Year - (int)((century + 19) * 100);
            JulianDate += (uint)(year * 1000);
            JulianDate += (uint)ParsedDate.DayOfYear;
            return JulianDate;
        } // DateStringToJulian

        /// <summary>
        /// Convert a JDE Julian date to a string date of the MM/DD/YYYY variety
        /// </summary>
        /// <param name="JulianDate"></param>
        /// <returns></returns>
        public static string JulianToDateString(uint JulianDate)
        {
            string DateString = String.Empty;
            int century = (int)Math.Truncate((decimal)JulianDate / 100000); // 0 = 19--, 1 = 20--, 2 = 21--, etc.
            int year = (int)JulianDate - (int)(century * 100000);
            year = (int)Math.Truncate((double)year / 1000);
            year += (int)((century + 19) * 100);
            DateTime date = new DateTime(year, 1, 1);
            date = date.AddDays((JulianDate % 1000) - 1);
            return $"{date.Month.ToString()}/{date.Day.ToString()}/{year.ToString()}";
        }
    }
}
