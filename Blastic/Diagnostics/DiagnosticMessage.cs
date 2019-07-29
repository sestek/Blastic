using System;
using System.Threading.Tasks;

namespace Blastic.Diagnostics
{
	public class DiagnosticMessage
	{
		public Severity Severity { get; }
		public string Message { get; }

		public Func<Task> Action { get; }
		public string ActionLabel { get; }

		public DiagnosticMessage(Severity severity, string message)
			:
			this(severity, message, null, null)
		{
		}

		public DiagnosticMessage(
			Severity severity,
			string message,
			Func<Task> action,
			string actionLabel)
		{
			Severity = severity;
			Message = message;
			Action = action;
			ActionLabel = actionLabel;
		}

		public async Task RunAction()
		{
			await Action();
		}
	}
}