using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class EventModel
	{
		private DateTime startDateAndTime;
		private DateTime finishDateAndTime;
		private string title;
		private string description;
		private int id;

		public EventModel()
		{

		}

		public EventModel(string title, DateTime start, DateTime finish, string description)
		{
			Title = title;
			StartDateAndTime = start;
			FinishDateAndTime = finish;
			Description = description;

		}
		
		public DateTime StartDateAndTime { get => startDateAndTime; set => startDateAndTime = value; }
		public DateTime FinishDateAndTime { get => finishDateAndTime; set => finishDateAndTime = value; }
		public string Title { get => title; set => title = value; }
		public string Description { get => description; set => description = value; }
		public int Id { get => id; set => id = value; }
	}
}
