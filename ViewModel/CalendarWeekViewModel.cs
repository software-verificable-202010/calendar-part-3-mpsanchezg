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
		private List<DateTime> daysOfCurrentWeek;
		private string currentMonth;
		private int currentYear;
		private CalendarWeekModel currentCalendarWeek;

		public CalendarWeekViewModel()
		{
			CurrentDay = DateTime.Now;
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
				NotifyPropertyChanged("CurrentDay");
			}
		}
		public List<DateTime> DaysOfCurrentWeek
		{ 
			get => daysOfCurrentWeek;
			set 
			{ 
				daysOfCurrentWeek = value;
				ChangeMonthOfWeek();
				ChangeYearOfWeek();
				NotifyPropertyChanged("DaysOfCurrentWeek");
			} 
		}
		public CalendarWeekModel CurrentCalendarWeek
		{
			get => currentCalendarWeek;
			set
			{
				currentCalendarWeek = value;
				NotifyPropertyChanged("CurrentCalendarWeek");
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


		public RelayCommand GoToNextWeekCommand { get; }
		private bool CanGoToNextWeek()
		{
			return true;
		}
		private void OnGoToNextWeek()
		{
			CurrentDay = CurrentDay.AddDays(Constants.DaysOfAWeek);
			DaysOfCurrentWeek = GetOtherDaysOfWeekByADay(CurrentDay);
		}

		public RelayCommand GoToLastWeekCommand { get; }
		private bool CanGoToLastWeek()
		{
			return true;
		}
		private void OnGoToLastWeek()
		{
			CurrentDay = CurrentDay.AddDays(-Constants.DaysOfAWeek);
			DaysOfCurrentWeek = GetOtherDaysOfWeekByADay(CurrentDay);
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
			int monthOfThursday = DaysOfCurrentWeek[Constants.Thursday - Constants.OneDay].Month;
			CurrentMonth = Constants.MonthNames[monthOfThursday - Constants.OneMonth];
		}
		private void ChangeYearOfWeek()
		{
			CurrentYear = DaysOfCurrentWeek[Constants.Thursday - Constants.OneDay].Year;
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
