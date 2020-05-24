using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class EventContext : DbContext
	{
		private const string ConnectionString = "Data Source=calendar_events.db";

		public DbSet<EventModel> Events { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite(ConnectionString);
	}
}
