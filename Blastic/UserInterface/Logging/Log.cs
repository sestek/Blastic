using Serilog.Events;

namespace Blastic.UserInterface.Logging
{
	public class Log
	{
		public string Date { get; set; }
		public LogEventLevel Level { get; set; }
		public string Source { get; set; }
		public string Message { get; set; }
	}
}