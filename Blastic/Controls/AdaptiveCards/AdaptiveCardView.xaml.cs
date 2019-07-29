using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Wpf;
using Bindables;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Blastic.Controls.AdaptiveCards
{
	public partial class AdaptiveCardView
	{
		private RenderedAdaptiveCard _renderedCard;

		public AdaptiveCardRenderer Renderer { get; }

		[DependencyProperty(IsReadOnly = true)]
		public FrameworkElement RenderedCardContent { get; private set; }

		[DependencyProperty(OnPropertyChanged = nameof(OnAdaptiveCardChanged))]
		public AdaptiveCard AdaptiveCard { get; set; }

		public AdaptiveCardView()
		{
			InitializeComponent();

			Loaded += (sender, args) =>
			{
				CommandBinding binding = new CommandBinding(NavigationCommands.GoToPage, GoToPage, CanGoToPage);
				CommandManager.RegisterClassCommandBinding(typeof(AdaptiveCardView), binding);
			};

			Renderer = new AdaptiveCardRenderer
			{
				Resources = LoadResources(),
				HostConfig = LoadHostConfig()
			};

			Renderer.ElementRenderers.Set<AdaptiveDateInput>(DateInputRenderer.Render);
			Renderer.ElementRenderers.Set<AdaptiveTimeInput>(TimeInputRenderer.Render);
		}

		private void GoToPage(object sender, ExecutedRoutedEventArgs e)
		{
			if (!(e.Parameter is string name))
			{
				return;
			}

			if (!string.IsNullOrWhiteSpace(name))
			{
				Process.Start(name);
			}
		}

		private void CanGoToPage(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private static void OnAdaptiveCardChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(e.NewValue is AdaptiveCard adaptiveCard))
			{
				return;
			}

			AdaptiveCardView adaptiveCardView = (AdaptiveCardView) d;
			adaptiveCardView.RenderCard(adaptiveCard);
		}

		private void RenderCard(AdaptiveCard adaptiveCard)
		{
			if (_renderedCard != null)
			{
				_renderedCard.OnAction -= OnCardAction;
			}

			if (adaptiveCard == null)
			{
				_renderedCard = null;
				RenderedCardContent = null;

				return;
			}

			_renderedCard = Renderer.RenderCard(adaptiveCard);
			_renderedCard.OnAction += OnCardAction;

			RenderedCardContent = _renderedCard.FrameworkElement;
		}

		private void OnCardAction(RenderedAdaptiveCard sender, AdaptiveActionEventArgs e)
		{
			if (e.Action is AdaptiveOpenUrlAction openUrlAction)
			{
				Process.Start(openUrlAction.Url.AbsoluteUri);
				return;
			}

			if (e.Action is AdaptiveSubmitAction submitAction)
			{
				JObject inputs = sender.UserInputs.AsJson();

				// Merge the Action.Submit Data property with the inputs
				inputs.Merge(submitAction.Data);

				MessageBox.Show(JsonConvert.SerializeObject(inputs, Formatting.Indented), "SubmitAction");
			}

			// TODO: Redirect actions to viewmodels.
			//else if (e.Action is AdaptiveShowCardAction showCardAction)
			//{
			//	// Action.ShowCard can be rendered inline automatically, or in "popup" mode
			//	// If the Host Config is set to Popup mode, then the app needs to show it
			//	if (Renderer.HostConfig.Actions.ShowCard.ActionMode == ShowCardActionMode.Popup)
			//	{
			//		var dialog = new ShowCardWindow(showCardAction.Title, showCardAction, Resources);
			//		dialog.Owner = this;
			//		dialog.ShowDialog();
			//	}
			//}
		}

		private ResourceDictionary LoadResources()
		{
			return new ResourceDictionary
			{
				Source = new Uri("pack://application:,,,/Blastic;component/Themes/AdaptiveCards.xaml", UriKind.RelativeOrAbsolute)
			};
		}

		private AdaptiveHostConfig LoadHostConfig()
		{
			Assembly assembly = typeof(AdaptiveCardView).Assembly;

			using (Stream stream = assembly.GetManifestResourceStream("Blastic.Themes.AdaptiveCardConfig.json"))
			using (StreamReader reader = new StreamReader(stream))
			{
				return AdaptiveHostConfig.FromJson(reader.ReadToEnd());
			}
		}
	}
}