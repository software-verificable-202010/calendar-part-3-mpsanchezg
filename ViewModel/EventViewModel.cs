using CalendarApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalendarApp.ViewModel
{
	public class EventViewModel : INotifyPropertyChanged
	{
		private string title;
		private string description;
		private DateTime startDateAndTime;
		private DateTime finishDateAndTime;
		private const string finishDateAndTimeProperty = "FinishDateAndTime";
		private const string startDateAndTimeProperty = "StartDateAndTime";
		private const string descriptionProperty = "Description";
		private const string titleProperty = "Title";
		public EventModelContext db;

		public EventViewModel()
		{
			db = new EventModelContext();
			StartDateAndTime = DateTime.Today;
			FinishDateAndTime = DateTime.Today;
			CreateEventCommand = new RelayCommand(OnCreateEvent, CanCreateEvent);
		}

		public DateTime FinishDateAndTime 
		{ 
			get => finishDateAndTime; 
			set 
			{ 
				finishDateAndTime = value; 
				NotifyPropertyChanged(finishDateAndTimeProperty); 
			} 
		}
		public DateTime StartDateAndTime 
		{ 
			get => startDateAndTime; 
			set{ 
				startDateAndTime = value; 
				NotifyPropertyChanged(startDateAndTimeProperty); 
			} 
		}
		public string Description 
		{ 
			get => description;
			set
			{
				description = value;
				NotifyPropertyChanged(descriptionProperty);
			}
		}
		public string Title 
		{ 
			get => title; 
			set
			{
				title = value;
				NotifyPropertyChanged(titleProperty);
			}
		}

		
		public RelayCommand CreateEventCommand { get; }
		private bool CanCreateEvent()
		{
			return true;
		}
		private void OnCreateEvent()
		{
			const string messageBoxTitle = "Alerta.";
			if (AreValidData(Title, StartDateAndTime, FinishDateAndTime))
			{
				CreateEvent(Title, StartDateAndTime, FinishDateAndTime, Description);
				MessageBox.Show(Constants.SuccessfulEvent, messageBoxTitle, MessageBoxButton.OK);
				return;
			}
			MessageBox.Show(Constants.FailedEvent, messageBoxTitle, MessageBoxButton.OK);
		}

		private void CreateEvent(string title, DateTime startDateAndTime, DateTime finishDateAndTime, string description)
		{
			EventModel eventModel = new EventModel(title, startDateAndTime, finishDateAndTime, description);
			db.Add(eventModel);
			db.SaveChanges();
		}
		private bool AreValidData(string title, DateTime startDateAndTime, DateTime finishDateAndTime)
		{
			return startDateAndTime < finishDateAndTime && title != null;
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
