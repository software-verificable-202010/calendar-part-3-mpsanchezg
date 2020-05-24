using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public static class Constants
	{
		public static readonly int DaysOfAWeek = 7;
		public static readonly int FirstElement = 0;
		public static readonly int StartDayNumber = 0;
		public static readonly int NumberOfMonthsInAYear = 12;
		public static readonly int OneDay = 1;
		public static readonly int OneMonth = 1;
		public static readonly int OneYear = 1;
		public static readonly int January = 1;
		public static readonly int December = 12;
        private static List<string> monthNames = new List<string>(NumberOfMonthsInAYear)
        {
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre",
            "Octubre",
            "Noviembre",
            "Diciembre"
        };
        public static readonly int Thursday = 4;

        public static List<string> MonthNames { get => monthNames; set => monthNames = value; }
    }
}
