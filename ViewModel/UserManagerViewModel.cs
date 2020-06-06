using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CalendarApp.Model;
using CalendarApp.View;
using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.ViewModel
{
	public class UserManagerViewModel : ViewModelBase
	{
		private CalendarModelContext db;
		private UserModel currentUser;
		private string loginUserName;
		private const string UserNameProperty = "LoginUserName";
		private const string CurrentUserProperty = "CurrentUser";


		public UserManagerViewModel()
		{
			db = new CalendarModelContext();
			LoginCommand = new RelayCommand(OnLogin, CanLogin);
		}

		public UserModel CurrentUser
		{
			get => currentUser;
			set
			{
				if (value != currentUser)
				{
					currentUser = value;
					// TODO: change the static current user to a dynamic
					Constants.CurrentUser = CurrentUser;
					NotifyPropertyChanged(CurrentUserProperty);
				}
				
			}
		}

		public string LoginUserName
		{
			get => loginUserName;
			set
			{
				loginUserName = value;
				NotifyPropertyChanged(UserNameProperty);
			}
		}

		public RelayCommand LoginCommand
		{
			get;
		}

		private bool CanLogin()
		{
			return true;
		}

		private void OnLogin()
		{
			const string messageBoxTitle = "Alerta.";
			if (IsValidUsername(loginUserName))
			{
				CurrentUser = db.Users.Include(u => u.Events).First(u=>u.UserName == LoginUserName);
				GoToCalendar();
				return;
			}
			MessageBox.Show(Constants.FailedLogin, messageBoxTitle, MessageBoxButton.OK);
			return;
		}

		private bool IsValidUsername(string username)
		{
			var allUsers = db.Users.ToList();
			for (var userIndex = 0; userIndex < allUsers.Count; userIndex++)
			{
				if (allUsers[userIndex].UserName == username)
				{
					return true;
				}
			}
			return false;
		}

		private void GoToCalendar()
		{
			CalendarView calendarDayView = new CalendarView();
			calendarDayView.Show();
			foreach (Window window in Application.Current.Windows)
			{
				if (window.Title == Constants.MainWindow)
				{
					window.Close();
				}
			}
		}

		

	}
}
