using CalendarApp.Model;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		private string invitedUser;
		private List<EventModel> userEvents;
		private ObservableCollection<UserModel> allUsers;
		private ObservableCollection<UserModel> invitedUsers;
		private const string FinishDateAndTimeProperty = "FinishDateAndTime";
		private const string StartDateAndTimeProperty = "StartDateAndTime";
		private const string DescriptionProperty = "Description";
		private const string TitleProperty = "Title";
		private const string CurrentUserProperty = "CurrentUser";
		private const string UserEventsProperty = "UserEvents";
		private const string InvitedUsersProperty = "InvitedUsers";
		private const string AllUserEventsProperty = "AllUserEvents";
		private const string InvitedUserProperty = "InvitedUser";
		private CalendarModelContext db;

		public EventViewModel()
		{
			db = new CalendarModelContext();
			StartDateAndTime = DateTime.Today;
			FinishDateAndTime = DateTime.Today;
			// TODO: change the static variable current user
			CurrentUser = Constants.CurrentUser;
			CurrentUserEvents = GetUserEvents(CurrentUser);
			AllUsers = new ObservableCollection<UserModel>(GetAllUsers());
			InvitedUsers = new ObservableCollection<UserModel>();
			CreateEventCommand = new RelayCommand(OnCreateEvent, CanCreateEvent);
			AddInvitedUsersCommand = new RelayCommand(OnInviteUsers, CanInviteUsers);
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
			set
			{
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
		public string InvitedUser
		{
			get => invitedUser;
			set
			{
				if (value != invitedUser)
				{
					invitedUser = value;
					NotifyPropertyChanged(InvitedUserProperty);
				}
			}
		}
		public ObservableCollection<UserModel> AllUsers
		{
			get => allUsers;
			set
			{
				allUsers = value;
				NotifyPropertyChanged(AllUserEventsProperty);
			}
		}
		public ObservableCollection<UserModel> InvitedUsers
		{
			get => invitedUsers;
			set
			{
				invitedUsers = value;
				NotifyPropertyChanged(InvitedUsersProperty);
			}
		}

		public RelayCommand CreateEventCommand
		{
			get;
		} 
		public RelayCommand AddInvitedUsersCommand
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
			UserModel currentUserCopy = db.Users.Include(u => u.UserEvents).ThenInclude(ue => ue.Event)
				.First(u => u.UserName == CurrentUser.UserName);
			var newEvent = new EventModel(currentUserCopy, title, startDateAndTime, finishDateAndTime, description);
			UserEventModel newUserEvents = new UserEventModel(currentUserCopy, newEvent);
			db.Add(newUserEvents);
			db.SaveChanges();
			InviteOtherUsers(newEvent);
			CurrentUser = currentUserCopy;
		}
		private void InviteOtherUsers(EventModel eventModel)
		{
			if (InvitedUsers.Count > 0)
			{
				foreach (var invitedUser in InvitedUsers)
				{
					UserModel invitedUserCopy = db.Users.Include(u => u.UserEvents).ThenInclude(ue => ue.Event)
						.First(u => u.UserName == invitedUser.UserName);
					var newEvent = eventModel;
					UserEventModel newUserEvents = new UserEventModel(invitedUserCopy, newEvent);
					db.Add(newUserEvents);
					db.SaveChanges();

				}
			}
		}

		private bool CanInviteUsers()
		{
			return true;
		}
		private void OnInviteUsers()
		{
			UserModel invited = db.Users.First(u=>u.UserName==invitedUser);
			if (!InvitedUsers.Contains(invited))
			{
				InvitedUsers.Add(invited);
			}
		}



		private List<UserModel> GetAllUsers()
		{
			List<UserModel> users = new List<UserModel>();
			foreach (var user in db.Users.ToList())
			{
				if (user.UserName != CurrentUser.UserName)
				{
					users.Add(user);
				}
			}
			return users;
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
