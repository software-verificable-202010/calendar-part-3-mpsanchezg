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
		#region Private Variables
		private string title;
		private string description;
		private DateTime startDateAndTime;
		private DateTime finishDateAndTime;
		private UserModel currentUser;
		private string invitedUser;
		private List<EventModel> userEvents;
		private ObservableCollection<EventModel> currentUserEventsCollection;
		private ObservableCollection<UserModel> allUsers;
		private ObservableCollection<UserModel> invitedUsers;
		private int customEventId;
		private EventModel customEvent;
		private const string FinishDateAndTimeProperty = "FinishDateAndTime";
		private const string StartDateAndTimeProperty = "StartDateAndTime";
		private const string DescriptionProperty = "Description";
		private const string TitleProperty = "Title";
		private const string CurrentUserProperty = "CurrentUser";
		private const string UserEventsProperty = "UserEvents";
		private const string InvitedUsersProperty = "InvitedUsers";
		private const string AllUserEventsProperty = "AllUserEvents";
		private const string InvitedUserProperty = "InvitedUser";
		private const string CurrentUserEventsCollectionProperty = "CurrentUserEventsCollection";
		private const string CustomEventProperty = "CustomEvent";
		private const string CustomEventIdProperty = "CustomEventId";
		private CalendarModelContext db;
		#endregion

		public EventViewModel()
		{
			db = new CalendarModelContext();
			StartDateAndTime = DateTime.Today;
			FinishDateAndTime = DateTime.Today;
			// TODO: change the static variable current user
			CurrentUser = Constants.CurrentUser;
			CurrentUserEvents = GetUserEvents(CurrentUser);
			CurrentUserEventsCollection = new ObservableCollection<EventModel>(CurrentUserEvents);
			AllUsers = new ObservableCollection<UserModel>(GetAllUsers());
			InvitedUsers = new ObservableCollection<UserModel>();
			CreateEventCommand = new RelayCommand(OnCreateEvent, CanCreateEvent);
			AddInvitedUsersCommand = new RelayCommand(OnInviteUsers, CanInviteUsers);
			EditEventCommand = new RelayCommand(OnEditEvent, CanEditEvent);
			DeleteEventCommand = new RelayCommand(OnDeleteEvent, CanDeleteEvent);
		}

		#region Properties

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
		public ObservableCollection<EventModel> CurrentUserEventsCollection
		{
			get => currentUserEventsCollection;
			set
			{
				currentUserEventsCollection = value;
				NotifyPropertyChanged(CurrentUserEventsCollectionProperty);
			}
		}
		public EventModel CustomEvent
		{
			get => customEvent;
			set
			{
				customEvent = value;
				NotifyPropertyChanged(CustomEventProperty);
			}
		}
		public int CustomEventId
		{
			get => customEventId;
			set
			{
				customEventId = value;
				CustomEvent = GetEventById(customEventId);
				NotifyPropertyChanged(CustomEventIdProperty);
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
		public RelayCommand EditEventCommand
		{
			get;
		}
		public RelayCommand DeleteEventCommand
		{
			get;
		}

		#endregion

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
			CurrentUserEvents.Add(newEvent);
			CurrentUserEventsCollection.Add(newEvent);

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
			string messageBoxTitle = "Agregar Invitado.";
			if (InvitedUser == null)
			{
				MessageBox.Show(Constants.InvalidInvited, messageBoxTitle, MessageBoxButton.OK);
				return;
			}
			var invited = db.Users.First(u=>u.UserName==InvitedUser);
			if (!InvitedUsers.Contains(invited))
			{
				InvitedUsers.Add(invited);
			}

			MessageBox.Show(Constants.InvitedAdded, messageBoxTitle, MessageBoxButton.OK);
		}

		private bool CanEditEvent()
		{
			return true;
		}
		private void OnEditEvent()
		{
			string messageBoxTitle = "Editar Evento.";
			if (!CurrentUserIsOwner())
			{
				MessageBox.Show(Constants.NotOwnerEditEvent, messageBoxTitle, MessageBoxButton.OK);
				return;
			}
			if (AreValidData(CustomEvent.Title, CustomEvent.StartDateAndTime, CustomEvent.FinishDateAndTime))
			{
				db.Events.Update(CustomEvent);
				db.SaveChanges();
				MessageBox.Show(Constants.SuccessfulEditEvent, messageBoxTitle, MessageBoxButton.OK);
				return;
			}
			MessageBox.Show(Constants.FailedEditEvent, messageBoxTitle, MessageBoxButton.OK);
			return;
		}
		
		private bool CanDeleteEvent()
		{
			return true;
		}
		private void OnDeleteEvent()
		{
			string messageBoxTitle = "Eliminar Evento.";
			if (CurrentUserIsOwner())
			{
				CurrentUserEvents.Remove(CustomEvent);
				CurrentUserEventsCollection.Remove(CustomEvent);
				var eventToDelete = db.Events.Where(e => e.Id == CustomEventId);
				db.RemoveRange(eventToDelete);
				db.SaveChanges();
			}

			MessageBox.Show(Constants.NotOwnerDeleteEvent, messageBoxTitle, MessageBoxButton.OK);
		}

		private bool CurrentUserIsOwner()
		{
			return CustomEvent.Owner.UserName == CurrentUser.UserName;
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
		private EventModel GetEventById(int id)
		{
			return db.Events.First(e=> e.Id == id);
		}

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
