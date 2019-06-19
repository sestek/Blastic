using System;
using PropertyChanged;

namespace Blastic.Initialization
{
	[AddINotifyPropertyChangedInterface]
	public class ProductInformation
	{
		public string ProgramName { get; set; }
		public Version Version { get; set; }
	}
}