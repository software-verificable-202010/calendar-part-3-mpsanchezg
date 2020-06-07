﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CalendarApp.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.ViewModel
{
	class SignUpViewModel : INotifyPropertyChanged
	{
		#region Private Variables
		private string userName;
		private readonly CalendarModelContext db;
		private const string UserNameProperty = "UserName";
		#endregion

		public SignUpViewModel()
		{
			db = new CalendarModelContext();
			CreateUserCommand = new RelayCommand(OnCreateUser, CanCreateUser);
		}

		#region Properties
		public string UserName
		{
			get => userName;
			set
			{
				userName = value;
				NotifyPropertyChanged(UserNameProperty);
			}
		}
		public RelayCommand CreateUserCommand
		{
			get;
		}
		#endregion

		#region Private Methods
		private bool CanCreateUser()
		{ 
			return true;
		}
		private void OnCreateUser()
		{
			const string messageBoxTitle = "Alerta.";
			if (IsValidUsername(UserName))
			{
				CreateUser(UserName);
				MessageBox.Show(Constants.SuccessfulUser, messageBoxTitle, MessageBoxButton.OK);
				return;
			}
			MessageBox.Show(Constants.FailedUser, messageBoxTitle, MessageBoxButton.OK);
			return;
		}

		private void CreateUser(string username)
		{
			UserModel newUser = new UserModel(username);
			db.Users.Add(newUser);
			db.SaveChanges();
		}
		private bool IsValidUsername(string username)
		{
			var dbUsers = db.Users;
			if (dbUsers.Any())
			{
				var allUsers = dbUsers.ToList();
				foreach (var user in allUsers)
				{
					if (user.UserName == username)
					{
						return false;
					}
				}
			}
			return true;
		}
		
		
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
		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

	}
}
