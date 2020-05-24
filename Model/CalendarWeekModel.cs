using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class CalendarWeekModel
	{
		private int year;
		private int monthNumber;
		private DateTime selectedDateOfWeek;
		private List<DateTime> daysOfWeek;

		public CalendarWeekModel(DateTime selectedDateOfWeek)
		{
			SelectedDateOfWeek = selectedDateOfWeek;
			MonthNumber = SelectedDateOfWeek.Month;
			Year = SelectedDateOfWeek.Year;
			DaysOfWeek = GetDaysOfWeekBySelectedDay(SelectedDateOfWeek);
		}

		public int Year { get => year; set => year = value; }
		public int MonthNumber { get => monthNumber; set => monthNumber = value; }
		public DateTime SelectedDateOfWeek { get => selectedDateOfWeek; set => selectedDateOfWeek = value; }
		public List<DateTime> DaysOfWeek { get => daysOfWeek; set => daysOfWeek = value; }

		private List<DateTime> GetDaysOfWeekBySelectedDay(DateTime selectedDateOfWeek)
		{
			List<DateTime> daysOfWeekBySelectedDay;
			int currentDayOfWeek = (int) selectedDateOfWeek.DayOfWeek;
			DateTime sunday = selectedDateOfWeek.AddDays(-currentDayOfWeek);
			DateTime monday = GetMonday(sunday, currentDayOfWeek);
			daysOfWeekBySelectedDay = Enumerable.Range(
				Constants.StartDayNumber, 
				Constants.DaysOfAWeek
				).Select(days => monday.AddDays(days)).ToList();

			return daysOfWeekBySelectedDay;
		}
		private DateTime GetMonday(DateTime sunday, int currentDayOfWeek)
		{
			DateTime monday = sunday.AddDays(Constants.OneDay);
			if (currentDayOfWeek == Constants.StartDayNumber)
			{
				return monday.AddDays(-Constants.DaysOfAWeek);
			}
			return monday;
		}
	}
}
