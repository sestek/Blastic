﻿using System.Threading;
using System.Threading.Tasks;
using PropertyChanged;
using Blastic.Services.Settings;
using Forge.Forms.Annotations;
using MaterialDesignThemes.Wpf;

namespace Blastic.UserInterface.Settings
{
	/// <summary>
	/// An individual setting.
	/// </summary>
	/// <typeparam name="T">Type of the value. Should be a primitive type.</typeparam>
	[AddINotifyPropertyChangedInterface]
	[Form(Mode = DefaultFields.None)]
	public class Setting<T>
	{
		private readonly ISettingsService _settingsService;

		/// <summary>
		/// Key to be used when writing to the database.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Label of the string to be shown on UI.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Help string to show on UI.
		/// </summary>
		public string Help { get; set; }

		/// <summary>
		/// Icon to show.
		/// </summary>
		public PackIconKind? Icon { get; set; }

		/// <summary>
		/// Default value to be returned when key does not exist in database.
		/// </summary>
		public T DefaultValue { get; }

		/// <summary>
		/// This property will be bound to the setting UI. Use this property while
		/// checking for errors.
		/// </summary>
		[Field(
			Name = "{Binding " + nameof(Label) + "}",
			Icon = "{Binding " + nameof(Icon) + "}")]
		public T SettingValue { get; set; }

		/// <summary>
		/// Use this property to check for the effective value of the setting.
		/// </summary>
		public T Value { get; set; }

		public Setting(
			ISettingsService settingsService,
			string key,
			T defaultValue)
		{
			_settingsService = settingsService;

			Key = key;
			DefaultValue = defaultValue;

			Value = DefaultValue;
			SettingValue = DefaultValue;
		}

		public async Task Read(CancellationToken cancellationToken)
		{
			Value = await _settingsService.Get(Key, DefaultValue, cancellationToken);
			SettingValue = Value;

			await AfterRead(cancellationToken);
		}

		public async Task Save(CancellationToken cancellationToken)
		{
			await BeforeSave(cancellationToken);

			await _settingsService.Put(Key, SettingValue, cancellationToken);
			Value = SettingValue;
		}

		public virtual Task AfterRead(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public virtual Task BeforeSave(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Revert()
		{
			SettingValue = Value;
		}

		public virtual string CheckError()
		{
			return null;
		}
	}
}