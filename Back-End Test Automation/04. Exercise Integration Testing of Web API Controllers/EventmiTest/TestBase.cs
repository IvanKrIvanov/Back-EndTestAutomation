using Eventmi.Infrastructure.Data.Contexts;
using Eventmi.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Internal.Execution;
using Event = Eventmi.Infrastructure.Models.Event;

namespace Eventmi.Tests
{
	public class TestsBase
	{

		private Event GetEventByName(string name)
		{
			var options = new DbContextOptionsBuilder<EventmiContext>().UseSqlServer("Server=.;Database=Eventmi;Trusted_Connection=True;MultipleActiveResultSets=true").Options;

			using var context = new EventmiContext(options);
			return context.Events.FirstOrDefault(x => x.Name == name);
		}
	}
}