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
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.ViewModel
{
	public class EventViewModel : ViewModelBase
	{
		private string title;
		private string description;
		private DateTime startDateAndTime;
		private DateTime finishDateAndTime;
		private UserModel currentUser;
		private List<EventModel> userEvents;
		private const string FinishDateAndTimeProperty = "FinishDateAndTime";
		private const string StartDateAndTimeProperty = "StartDateAndTime";
		private const string DescriptionProperty = "Description";
		private const string TitleProperty = "Title";
		private const string CurrentUserProperty = "CurrentUser";
		private const string UserEventsProperty = "UserEvents";
		private CalendarModelContext db;

		public EventViewModel()
		{
			db = new CalendarModelContext();
			StartDateAndTime = DateTime.Today;
			FinishDateAndTime = DateTime.Today;
			// TODO: change the static variable current user
			CurrentUser = Constants.CurrentUser;
			CurrentUserEvents = GetUserEvents(CurrentUser);
			CreateEventCommand = new RelayCommand(OnCreateEvent, CanCreateEvent);
		}

		public DateTime FinishDateAndTime 
		{ 
			get => finishDateAndTime; 
			set 
			{ 
				finishDateAndTime = value; 
				NotifyPropertyChanged(FinishDateAndTimeProperty); 
			} 
		}
		public DateTime StartDateAndTime 
		{ 
			get => startDateAndTime; 
			set{ 
				startDateAndTime = value; 
				NotifyPropertyChanged(StartDateAndTimeProperty); 
			} 
		}
		public string Description 
		{ 
			get => description;
			set
			{
				description = value;
				NotifyPropertyChanged(DescriptionProperty);
			}
		}
		public string Title 
		{ 
			get => title; 
			set
			{
				title = value;
				NotifyPropertyChanged(TitleProperty);
			}
		}
		public List<EventModel> CurrentUserEvents
		{
			get => userEvents;
			set
			{
				userEvents = value;
				NotifyPropertyChanged(UserEventsProperty);
			}
		}
		public UserModel CurrentUser
		{
			get => currentUser;
			set
			{
				if (value != currentUser)
				{
					currentUser = value;
					NotifyPropertyChanged(CurrentUserProperty);
				}
			}
		}

		public RelayCommand CreateEventCommand
		{
			get;
		}
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
				CurrentUserEvents = GetUserEvents(CurrentUser);
				MessageBox.Show(Constants.SuccessfulEvent, CurrentUser.UserName, MessageBoxButton.OK);
				return;
			}
			MessageBox.Show(Constants.FailedEvent, messageBoxTitle, MessageBoxButton.OK);
		}

		private void CreateEvent(string title, DateTime startDateAndTime, DateTime finishDateAndTime, string description)
		{
			var newEvent= new EventModel(CurrentUser, title, startDateAndTime, finishDateAndTime, description);
			UserEventModel newUserEvents = new UserEventModel(CurrentUser, newEvent);
			CurrentUser.UserEvents.Add(newUserEvents);
			newEvent.UserEvents.Add(newUserEvents);
			db.Add(newEvent);
			db.Update(CurrentUser);
			db.Add(newUserEvents);
			db.SaveChanges();
		}

		private List<EventModel> GetUserEvents(UserModel user)
		{
			List<EventModel> userEvents = new List<EventModel>();
			int userEventsQuantity = user.UserEvents.Count();
			for (var eventIndex = Constants.FirstElement; eventIndex < userEventsQuantity; eventIndex++)
			{
				userEvents.Add(CurrentUser.UserEvents[eventIndex].Event);
			}
			return userEvents;
		}
		private bool AreValidData(string title, DateTime startDateAndTime, DateTime finishDateAndTime)
		{
			return startDateAndTime < finishDateAndTime && title != null;
		}

		// TODO: hacer que los usuarios puedan invitar a otros

		private UserManagerViewModel _children;
		public UserManagerViewModel children
		{
			get { return _children; }
			set
			{
				if (value != _children)
				{
					_children = value;
					PropertyChanged += ChildOnPropertyChanged;
					NotifyPropertyChanged("children");
				}
			}
		}
		private void ChildOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			CurrentUser = children.CurrentUser;
		}
	}
}
