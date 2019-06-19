using Forge.Forms;
using Forge.Forms.Annotations;

namespace Blastic.Forms
{
	[Form(Mode = DefaultFields.None)]
	[Title("{Binding Title}", IsVisible = "{Binding Title|IsNotEmpty}")]
	[Text("{Binding Message}", IsVisible = "{Binding Message|IsNotEmpty}")]
	[PositiveAction]
	[NegativeAction]
	public class BlasticFormBase : DialogBase
	{
	}
}