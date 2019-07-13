using System;
using System.Windows;
using System.Windows.Controls;
using AdaptiveCards;
using AdaptiveCards.Rendering.Wpf;

namespace Blastic.Controls.AdaptiveCards
{
	public static class DateInputRenderer
	{
		public static FrameworkElement Render(AdaptiveDateInput input, AdaptiveRenderContext context)
		{
			if (!context.Config.SupportsInteractivity)
			{
				AdaptiveTextBlock textBlock = AdaptiveTypedElementConverter.CreateElement<AdaptiveTextBlock>();
				textBlock.Text = XamlUtilities.GetFallbackText(input) ?? input.Placeholder;

				return context.Render(textBlock);
			}

			DatePicker datePicker = new DatePicker
			{
				DataContext = input,
				ToolTip = input.Placeholder,
				Style = context.GetStyle("Adaptive.Input.Date")
			};


			if (DateTime.TryParse(input.Value, out DateTime value))
			{
				datePicker.SelectedDate = value;
			}

			if (DateTime.TryParse(input.Min, out DateTime minValue))
			{
				datePicker.DisplayDateStart = minValue;
			}

			if (DateTime.TryParse(input.Max, out DateTime maxValue))
			{
				datePicker.DisplayDateEnd = maxValue;
			}

			context.InputBindings.Add(input.Id, () => datePicker.SelectedDate?.ToString("yyyy-MM-dd") ?? "");

			return datePicker;
		}
	}
}