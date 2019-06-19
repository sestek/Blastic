using Forge.Forms.Annotations;

namespace Blastic.Forms
{
	public class PositiveActionAttribute : ActionAttribute
	{
		public PositiveActionAttribute() : base("positive", "{Binding PositiveAction}")
		{
			IsDefault = true;
			ClosesDialog = true;
			IsVisible = "{Binding PositiveAction|IsNotEmpty}";
			Icon = "{Binding PositiveActionIcon}";
		}
	}
}