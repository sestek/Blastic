using System;
using System.ComponentModel;
using System.Threading.Tasks;
using PropertyChanged;

namespace Blastic.Controls.RequestField
{
	[AddINotifyPropertyChangedInterface]
	public class RequestFieldViewModel : IDataErrorInfo
	{
		private TaskCompletionSource<bool> _taskCompletionSource;

		public bool IsOpen { get; set; }

		public string Mask { get; set; }
		public Func<string, string> ValueChecker { get; set; }

		public bool IsMultiline { get; set; }

		[AlsoNotifyFor(nameof(CanOk))]
		public string Field { get; set; }
		public string Explanation { get; set; }
		public string DefaultValue { get; set; }

		public Task<bool> Show()
		{
			_taskCompletionSource = new TaskCompletionSource<bool>();

			Field = DefaultValue;
			IsOpen = true;

			return _taskCompletionSource.Task;
		}

		public void Cancel()
		{
			if (!IsOpen)
			{
				return;
			}

			IsOpen = false;

			_taskCompletionSource?.SetResult(false);
			_taskCompletionSource = null;
		}

		public bool CanOk => !string.IsNullOrWhiteSpace(Field);
		public void Ok()
		{
			if (!IsOpen)
			{
				return;
			}

			IsOpen = false;

			_taskCompletionSource?.SetResult(true);
			_taskCompletionSource = null;
		}

		public void OkNotDisabled()
		{
			if (IsMultiline || !CanOk)
			{
				return;
			}

			Ok();
		}

		public string this[string name]
		{
			get
			{
				if (name != nameof(Field))
				{
					return null;
				}

				if (string.IsNullOrWhiteSpace(Field))
				{
					return "";
				}

				return ValueChecker?.Invoke(Field);
			}
		}

		public string Error => null;
	}
}