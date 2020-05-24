using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class CalendarModel 
	{
		private CalendarMonthModel calendarMonthModel;

		public CalendarModel(int year, int monthNumber)
		{
			CalendarMonthModel = new CalendarMonthModel(monthNumber, year);
		}

		public CalendarMonthModel CalendarMonthModel { get => calendarMonthModel; set => calendarMonthModel = value; }

	}
}
