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
        public static readonly List<string> MonthNames = new List<string>(NumberOfMonthsInAYear)
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
        public static readonly string ColorOfWeekend = "#FF08E8DE";
        public static readonly string ColorOfWeek = "#FFAFEEEE";
        public static readonly string ColorOfToday = "#FF00CED1";
        public static readonly string ColorOfDaysOfOtherMonth = "#FFD7F3F3";
        public static readonly string SuccessfulEvent = "¡Evento creado con éxito!";
        public static readonly string FailedEvent = "Error. Revise que el evento tenga título y que las fechas sean correctas.";
        public static readonly string SuccessfulUser = "¡Usuario creado con éxito!";
        public static readonly string FailedUser = "El nombre de usuario ya existe. Por favor, intente con otro nombre.";
        public static readonly string FailedLogin = "Nombre de usuario inválido.";
        public static readonly string MainWindow = "MainWindow";
        public static UserModel CurrentUser;
	}
}
