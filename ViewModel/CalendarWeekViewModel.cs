using CalendarApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CalendarApp.ViewModel
{
	public class CalendarWeekViewModel : INotifyPropertyChanged
	{
		private DateTime currentDay;
		private List<CalendarDayModel> daysOfCurrentWeek;
		private string currentMonth;
		private int currentYear;
		private CalendarWeekModel currentCalendarWeek;
		private RelayCommand goToNextWeekCommand;
		private RelayCommand goToLastWeekCommand;
		private const string currentDayProperty = "CurrentDay";
		private const string currentCalendarWeekProperty = "CurrentCalendarWeek";
		private const string daysOfCurrentWeekProperty = "DaysOfCurrentWeek";

		public CalendarWeekViewModel()
		{
			CurrentDay = DateTime.Today;
			CurrentCalendarWeek = new CalendarWeekModel(CurrentDay);
			DaysOfCurrentWeek = CurrentCalendarWeek.DaysOfWeek;
			GoToNextWeekCommand = new RelayCommand(OnGoToNextWeek, CanGoToNextWeek);
			GoToLastWeekCommand = new RelayCommand(OnGoToLastWeek, CanGoToLastWeek);
		}

		public DateTime CurrentDay 
		{ 
			get => currentDay;
			set { 
				currentDay = value;
				NotifyPropertyChanged(currentDayProperty);
			}
		}
		public List<CalendarDayModel> DaysOfCurrentWeek
		{ 
			get => daysOfCurrentWeek;
			set 
			{ 
				daysOfCurrentWeek = value;
				ChangeMonthOfWeek();
				ChangeYearOfWeek();
				NotifyPropertyChanged(daysOfCurrentWeekProperty);
			} 
		}
		public CalendarWeekModel CurrentCalendarWeek
		{
			get => currentCalendarWeek;
			set
			{
				currentCalendarWeek = value;
				NotifyPropertyChanged(currentCalendarWeekProperty);
			}
		}
		public string CurrentMonth
		{
			get => currentMonth;
			set
			{
				currentMonth = value;
				NotifyPropertyChanged("CurrentMonth");
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
		public RelayCommand GoToNextWeekCommand
		{
			get => goToNextWeekCommand;
			set => goToNextWeekCommand = value;
		}
		public RelayCommand GoToLastWeekCommand
		{
			get => goToLastWeekCommand;
			set => goToLastWeekCommand = value;
		}

		private bool CanGoToNextWeek()
		{
			return true;
		}
		private void OnGoToNextWeek()
		{
			CurrentDay = CurrentDay.AddDays(Constants.DaysOfAWeek);
			DaysOfCurrentWeek = GetCalendarDaysOfWeek(CurrentDay);
		}

		private bool CanGoToLastWeek()
		{
			return true;
		}
		private void OnGoToLastWeek()
		{
			CurrentDay = CurrentDay.AddDays(-Constants.DaysOfAWeek);
			DaysOfCurrentWeek = GetCalendarDaysOfWeek(CurrentDay);
		}


		private List<CalendarDayModel> GetCalendarDaysOfWeek(DateTime day)
		{
			List<CalendarDayModel> calendarDaysOfWeek = new List<CalendarDayModel>();
			List<DateTime> daysOfWeek = GetOtherDaysOfWeekByADay(day);
			for (int dayOfWeekIndex = Constants.FirstElement; dayOfWeekIndex < Constants.DaysOfAWeek; dayOfWeekIndex++)
			{
				string colorOfCalendarDay = PutColorOfCalendarDayOfWeek(daysOfWeek[dayOfWeekIndex]);
				calendarDaysOfWeek.Add(new CalendarDayModel(daysOfWeek[dayOfWeekIndex], colorOfCalendarDay));
			}

			return calendarDaysOfWeek;
		}
		private string PutColorOfCalendarDayOfWeek(DateTime day)
		{
			if (IsToday(day))
			{
				return Constants.ColorOfToday;
			}
			else if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
			{
				return Constants.ColorOfWeek;
			}
			return Constants.ColorOfWeekend;
		}
		private bool IsToday(DateTime date)
		{
			return date == DateTime.Today;
		}
		private List<DateTime> GetOtherDaysOfWeekByADay(DateTime day)
		{
			List<DateTime> daysOfWeekBySelectedDay;
			int currentDayOfWeek = (int)day.DayOfWeek;
			DateTime sunday = day.AddDays(-currentDayOfWeek);
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

		private void ChangeMonthOfWeek()
		{
			int monthOfThursday = DaysOfCurrentWeek[Constants.Thursday - Constants.OneDay].Date.Month;
			CurrentMonth = Constants.MonthNames[monthOfThursday - Constants.OneMonth];
		}
		private void ChangeYearOfWeek()
		{
			CurrentYear = DaysOfCurrentWeek[Constants.Thursday - Constants.OneDay].Date.Year;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler)
			{
				PropertyChangedDelegate(propertyName, handler);
			}
		}
		private void PropertyChangedDelegate(string propertyName, PropertyChangedEventHandler handler)
		{
			handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
