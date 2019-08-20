using System;
using System.Windows;
using System.Windows.Input;
using Blastic.UserInterface.Settings;
using Caliburn.Micro;
using TriggerBase = Microsoft.Xaml.Behaviors.TriggerBase;

namespace Blastic.Caliburn
{
	public class CaliburnMicroInitializer
	{
		public static void Initialize()
		{
			Func<DependencyObject, string, TriggerBase> defaultCreateTrigger = Parser.CreateTrigger;

			Parser.CreateTrigger = (target, triggerText) =>
			{
				if (triggerText == null)
				{
					return defaultCreateTrigger(target, null);
				}

				string triggerDetail = triggerText
					.Replace("[", string.Empty)
					.Replace("]", string.Empty);

				string[] splits = triggerDetail.Split((char[]) null, StringSplitOptions.RemoveEmptyEntries);

				switch (splits[0])
				{
					case "Key":
						Key key = (Key) Enum.Parse(typeof(Key), splits[1], true);
						return new KeyTrigger { Key = key };

					case "Gesture":
						MultiKeyGesture gesture = (MultiKeyGesture) (new MultiKeyGestureConverter()).ConvertFrom(splits[1]);
						return new KeyTrigger { Modifiers = gesture.KeySequences[0].Modifiers, Key = gesture.KeySequences[0].Keys[0] };
				}

				return defaultCreateTrigger(target, triggerText);
			};

			Func<Type, DependencyObject, object, UIElement> defaultFunction = ViewLocator.LocateForModelType;

			ViewLocator.LocateForModelType = (type, o, arg) =>
			{
				if (typeof(ISettingsSectionViewModel).IsAssignableFrom(type))
				{
					return ViewLocator.GetOrCreateViewType(typeof(FormSettingSectionView));
				}

				return defaultFunction(type, o, arg);
			};
		}
	}
}