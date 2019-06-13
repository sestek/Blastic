namespace WpfTemplate.Diagnostics
{
	public class DiagnosticMessage
	{
		public Severity Severity { get; }
		public string Message { get; }

		public DiagnosticMessage(Severity severity, string message)
		{
			Severity = severity;
			Message = message;
		}
	}
}