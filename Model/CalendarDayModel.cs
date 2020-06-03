using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CalendarApp.Model
{
	public class CalendarDayModel
	{
		private DateTime date;
		private string color;

		public CalendarDayModel(DateTime date, string color)
		{
			Date = date;
			Color = color;
		}

		public DateTime Date
		{
			get => date;
			set => date = value;
		}
		public string Color
		{
			get => color;
			set => color = value;
		}

	}
}
