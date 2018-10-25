using NLog;

namespace Yaver.Host.Wpf.UserInterface.Logging
{
	public class Log
	{
		public string Date { get; set; }
		public LogLevel Level { get; set; }
		public string Source { get; set; }
		public string Message { get; set; }
	}
}