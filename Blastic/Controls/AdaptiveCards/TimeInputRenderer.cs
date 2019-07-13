using System;
using System.Windows;
using AdaptiveCards;
using AdaptiveCards.Rendering.Wpf;
using MaterialDesignThemes.Wpf;

namespace Blastic.Controls.AdaptiveCards
{
	public class TimeInputRenderer
	{
		public static FrameworkElement Render(AdaptiveTimeInput input, AdaptiveRenderContext context)
		{
			if (!context.Config.SupportsInteractivity)
			{
				AdaptiveTextBlock textBlock = AdaptiveTypedElementConverter.CreateElement<AdaptiveTextBlock>();
				textBlock.Text = XamlUtilities.GetFallbackText(input) ?? input.Placeholder;

				return context.Render(textBlock);
			}

			TimePicker timePicker = new TimePicker
			{
				DataContext = input,
				ToolTip = input.Placeholder,
				Style = context.GetStyle("Adaptive.Input.Time"),
				Is24Hours = true
			};

			if (DateTime.TryParse(input.Value, out DateTime value))
			{
				timePicker.SelectedTime = value;
			}

			context.InputBindings.Add(input.Id, () => timePicker.SelectedTime?.ToString("HH:mm") ?? "");

			return timePicker;
		}
	}
}