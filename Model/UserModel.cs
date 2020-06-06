using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class UserModel
	{
		private int id;
		private string userName;
		private List<EventModel> events;

		public UserModel()
		{

		}
		public UserModel(string username)
		{
			UserName = username;
		}

		public int Id
		{
			get => id;
			set => id = value;
		}

		public string UserName
		{
			get => userName;
			set => userName = value;
		}

		public List<EventModel> Events
		{
			get => events;
			set => events = value;
		}
	}
}
