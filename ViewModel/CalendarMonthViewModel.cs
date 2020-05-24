using CalendarApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarApp.ViewModel
{
	public class CalendarMonthViewModel : INotifyPropertyChanged
	{
		private DateTime currentDate;
		private int currentMonth;
		private string currentMonthName;
		private int currentYear;
		private CalendarMonthModel currentCalendarMonth;
		private List<DateTime> daysOfCurrentMonth;

		public CalendarMonthViewModel()
		{			
			CurrentDate = DateTime.Now;
			CurrentMonth = CurrentDate.Month;
			CurrentYear = CurrentDate.Year;
			CurrentCalendarMonth = new CalendarMonthModel(CurrentMonth, CurrentYear);
			GoToNextMonthCommand = new RelayCommand(OnGoToNextMonth, CanGoToNextMonth);
			GoToLastMonthCommand = new RelayCommand(OnGoToLastMonth, CanGoToLastMonth);
		}

		public DateTime CurrentDate { get => currentDate; set =>currentDate = value; }
		public int CurrentMonth { 
			get => currentMonth; 
			set 
			{ 
				currentMonth = GetCorrectNumberOfMonth(value);
				CurrentMonthName = Constants.MonthNames[currentMonth - Constants.OneMonth];
				NotifyPropertyChanged("CurrentMonth");
			}
		}
		public string CurrentMonthName 
		{ 
			get => currentMonthName;
			set { 
				currentMonthName = value;
				NotifyPropertyChanged("CurrentMonthName");
			}
		}
		public int CurrentYear 
		{ 
			get => currentYear;
			set 
			{ 
				currentYear = value;
				NotifyPropertyChanged("CurrentYear");

			}
		}
		public List<DateTime> DaysOfCurrentMonth 
		{ 
			get => daysOfCurrentMonth;
			set 
			{ 
				daysOfCurrentMonth = value;
				NotifyPropertyChanged("DaysOfCurrentMonth");
			}
		}
		public CalendarMonthModel CurrentCalendarMonth
		{
			get => currentCalendarMonth;
			set
			{
				currentCalendarMonth = value;
				CurrentMonth = currentCalendarMonth.MonthNumber;
				CurrentYear = currentCalendarMonth.YearOfMonth;
				SetMissingLastDaysOfWeekToCalendarMonth(currentCalendarMonth);
				NotifyPropertyChanged("CurrentCalendarMonth");

			}
		}

		private void SetMissingLastDaysOfWeekToCalendarMonth(CalendarMonthModel calendarMonthModel)
		{
			DateTime firstDayOfMonth = calendarMonthModel.DaysOfMonth.First();
			if (FirstDayIsMonday(firstDayOfMonth))
			{
				DaysOfCurrentMonth = currentCalendarMonth.DaysOfMonth;
				return;
			}
			DaysOfCurrentMonth = AddDaysOfLastMonth(calendarMonthModel);
			return;
		}
		private bool FirstDayIsMonday(DateTime dateTime)
		{
			if (dateTime.DayOfWeek == DayOfWeek.Monday)
			{
				return true;
			}
			return false;
		}
		private List<DateTime> AddDaysOfLastMonth(CalendarMonthModel calendarMonthModel)
		{
			List<DateTime> daysOfCalendarMonth = calendarMonthModel.DaysOfMonth;
			DateTime firstDayOfDateTimes = daysOfCalendarMonth[Constants.FirstElement];
			int numberOfDayOfWeek = (int)firstDayOfDateTimes.DayOfWeek;
			int yearOfCalendarMonth = calendarMonthModel.YearOfMonth;
			int lastMonth = GetCorrectNumberOfMonth(calendarMonthModel.MonthNumber - Constants.OneMonth);
			int numberOfDayInLastMonth = DateTime.DaysInMonth(yearOfCalendarMonth, lastMonth);
			int numberOfMissingDays = GetNumberOfMissingDays(numberOfDayOfWeek);
			DateTime dayToAdd;
			int numberOfDayToAdd;

			for (int day = Constants.StartDayNumber; day < numberOfMissingDays; day++)
			{
				numberOfDayToAdd = numberOfDayInLastMonth - day;
				dayToAdd = new DateTime(yearOfCalendarMonth, lastMonth, numberOfDayToAdd);
				daysOfCalendarMonth.Insert(Constants.FirstElement, dayToAdd);
			}
			return daysOfCalendarMonth;
		}

		private int GetNumberOfMissingDays(int numberOfDayOfWeek)
		{
			if (numberOfDayOfWeek < (int)DayOfWeek.Monday)
			{
				return Constants.DaysOfAWeek - 1;
			}
			return numberOfDayOfWeek - 1;
		}
		private int GetCorrectNumberOfMonth(int numberOfMonth)
		{
			int monthNumber = numberOfMonth % Constants.NumberOfMonthsInAYear;
			if (monthNumber == 0)
			{
				return Constants.December;
			}
			return monthNumber;

		}

		
		public RelayCommand GoToNextMonthCommand { get; }
		private bool CanGoToNextMonth()
		{
			return true;
		}
		private void OnGoToNextMonth()
		{
			int nextMonth = GetCorrectNumberOfMonth(CurrentMonth + Constants.OneMonth);

			int yearOfNextMonth = CurrentYear;
			if (nextMonth == Constants.January)
			{
				yearOfNextMonth += Constants.OneYear;
			}
			CurrentCalendarMonth = new CalendarMonthModel(nextMonth, yearOfNextMonth);
		}

		public RelayCommand GoToLastMonthCommand { get; }

		private bool CanGoToLastMonth()
		{			
			return true;
		}
		private void OnGoToLastMonth()
		{
			int lastMonth = GetCorrectNumberOfMonth(CurrentMonth - Constants.OneMonth);
			int yearOfLastMonth = CurrentYear;
			if (lastMonth == Constants.December)
			{
				yearOfLastMonth -= Constants.OneYear;
			}
			CurrentCalendarMonth = new CalendarMonthModel(lastMonth, yearOfLastMonth);
		}

		
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler)
				PropertyChangedDelegate(propertyName, handler);
		}
		private void PropertyChangedDelegate(string propertyName, PropertyChangedEventHandler handler)
		{
			handler(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
