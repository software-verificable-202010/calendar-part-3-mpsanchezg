using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarApp.Model
{
	public class CalendarModelContext : DbContext
	{
		private const string ConnectionString = "Data Source=calendar.db";

		public DbSet<EventModel> Events
		{
			get; 
			set;
		}

		public DbSet<UserModel> Users
		{
			get;
			set;
		}

		public DbSet<UserEventModel> UserEvent
		{
			get;
			set;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite(ConnectionString);
	}
}
