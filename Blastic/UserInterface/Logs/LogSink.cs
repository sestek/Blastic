using System.Collections.Generic;
using Caliburn.Micro;
using Serilog.Core;
using Serilog.Events;

namespace Blastic.UserInterface.Logs
{
	public class LogSink : ILogEventSink
	{
		public IObservableCollection<Log> Logs { get; }

		public LogSink()
		{
			Logs = new BindableCollection<Log>();
		}

		public void Emit(LogEvent logEvent)
		{
			IReadOnlyDictionary<string, LogEventPropertyValue> properties = logEvent.Properties;

			string source = "";

			if (properties.TryGetValue("SourceContext", out LogEventPropertyValue sourceValue))
			{
				source = sourceValue.ToString();
			}

			Log log = new Log
			{
				Date = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
				Level = logEvent.Level,
				Source = source,
				Message = logEvent.RenderMessage()
			};

			Logs.Add(log);
		}
	}
}