namespace Blastic.Execution
{
	public interface IHasExecutionContext
	{
		ExecutionContext ExecutionContext { get; }
	}
}