using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class CalendarMonthModel
	{
		private int monthNumber;
		private List<DateTime> daysOfMonth;
		private int yearOfMonth;

		public CalendarMonthModel(int monthNumber, int year)
		{
			YearOfMonth = year;
			MonthNumber = monthNumber;

		}

		public int MonthNumber 
		{ 
			get => monthNumber;
			set 
			{ 
				monthNumber = value;
				daysOfMonth = SetDaysOfTheMonth(MonthNumber, YearOfMonth);
			}
		}

		public List<DateTime> DaysOfMonth
		{
			get => daysOfMonth;
			set => daysOfMonth = value;
		}

		public int YearOfMonth
		{
			get => yearOfMonth;
			set
			{
				yearOfMonth = value;
			}
		}


		private List<DateTime> SetDaysOfTheMonth(int month, int year)
		{
			List<DateTime> newDaysOfMonth = new List<DateTime>();
			int firstNumberOfDay = 1;
			int nDaysInMonth = DateTime.DaysInMonth(year, month);

			for (int dayNumber = firstNumberOfDay; dayNumber <= nDaysInMonth; dayNumber++)
			{
				newDaysOfMonth.Add(new DateTime(year, month, dayNumber));
			}

			return newDaysOfMonth;
		}
	}
}
