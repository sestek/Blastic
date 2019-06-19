using Forge.Forms.Annotations;

namespace Blastic.Forms
{
	public class NegativeActionAttribute : ActionAttribute
	{
		public NegativeActionAttribute() : base("negative", "{Binding NegativeAction}")
		{
			IsCancel = true;
			ClosesDialog = true;
			IsVisible = "{Binding NegativeAction|IsNotEmpty}";
			Icon = "{Binding NegativeActionIcon}";
		}
	}
}