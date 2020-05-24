using CalendarApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.ViewModel
{
	public class EventViewModel : INotifyPropertyChanged
	{
		private string title;
		private string description;
		private DateTime startDateAndTime;
		private DateTime finishDateAndTime;
		public EventContext db;

		public EventViewModel()
		{
			db = new EventContext();
			CreateEventCommand = new RelayCommand(OnCreateEvent, CanCreateEvent);
		}

		public DateTime FinishDateAndTime 
		{ 
			get => finishDateAndTime; 
			set 
			{ 
				finishDateAndTime = value; 
				NotifyPropertyChanged("FinishDateAndTime"); 
			} 
		}
		public DateTime StartDateAndTime 
		{ 
			get => startDateAndTime; 
			set{ 
				startDateAndTime = value; 
				NotifyPropertyChanged("StartDateAndTime"); 
			} 
		}
		public string Description 
		{ 
			get => description;
			set
			{
				description = value;
				NotifyPropertyChanged("Description");
			}
		}
		public string Title { get => title; set
			{
				title = value;
				NotifyPropertyChanged("Title");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(String propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (null != handler)
				PropertyChangedDelegate(propertyName, handler);
		}

		public RelayCommand CreateEventCommand { get; }
		private bool CanCreateEvent()
		{
			return true;
		}
		private void OnCreateEvent()
		{
			EventModel eventModel = new EventModel(Title, StartDateAndTime, FinishDateAndTime, Description);
			db.Add(eventModel);
			db.SaveChanges();
		}

		private void PropertyChangedDelegate(string propertyName, PropertyChangedEventHandler handler)
		{
			handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
