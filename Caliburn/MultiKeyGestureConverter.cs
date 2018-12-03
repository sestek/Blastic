using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;

namespace WpfTemplate.Caliburn
{
	/// <summary>
	///   Class used to define a converter for the <see cref="MultiKeyGesture" /> class.
	/// </summary>
	/// <remarks>
	///   At the moment it is able to convert strings like <c>Alt+K,R</c> in proper multi-key gestures.
	/// </remarks>
	public class MultiKeyGestureConverter : TypeConverter
	{
		/// <summary>
		///   The default instance of the converter.
		/// </summary>
		public static readonly MultiKeyGestureConverter DefaultConverter = new MultiKeyGestureConverter();

		/// <summary>
		///   The inner key converter.
		/// </summary>
		private static readonly KeyConverter KeyConverter = new KeyConverter();

		/// <summary>
		///   The inner modifier key converter.
		/// </summary>
		private static readonly ModifierKeysConverter ModifierKeysConverter = new ModifierKeysConverter();

		/// <summary>
		///   Tries to get the modifier equivalent to the specified string.
		/// </summary>
		/// <param name="str"> The string. </param>
		/// <param name="modifier"> The modifier. </param>
		/// <returns> <c>True</c> if a valid modifier was found; otherwise, <c>false</c> . </returns>
		private static bool TryGetModifierKeys(string str, out ModifierKeys modifier)
		{
			switch (str.ToUpperInvariant())
			{
				case "CONTROL":
				case "CTRL":
					modifier = ModifierKeys.Control;
					return true;
				case "SHIFT":
					modifier = ModifierKeys.Shift;
					return true;
				case "ALT":
					modifier = ModifierKeys.Alt;
					return true;
				case "WINDOWS":
				case "WIN":
					modifier = ModifierKeys.Windows;
					return true;
				default:
					modifier = ModifierKeys.None;
					return false;
			}
		}

		/// <summary>
		///   Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context"> An <see cref="System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="sourceType"> A <see cref="System.Type" /> that represents the type you want to convert from. </param>
		/// <returns> true if this converter can perform the conversion; otherwise, false. </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		/// <summary>
		///   Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context"> An <see cref="System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture"> The <see cref="System.Globalization.CultureInfo" /> to use as the current culture. </param>
		/// <param name="value"> The <see cref="object" /> to convert. </param>
		/// <returns> An <see cref="object" /> that represents the converted value. </returns>
		/// <exception cref="System.NotSupportedException">The conversion cannot be performed.</exception>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string str = (value as string);

			if (!string.IsNullOrEmpty(str))
			{
				string[] sequences = str.Split(',');

				List<KeySequence> keySequences = new List<KeySequence>();


				foreach (string sequence in sequences)
				{
					ModifierKeys modifier = ModifierKeys.None;
					List<Key> keys = new List<Key>();
					string[] keyStrings = sequence.Split('+');
					int modifiersCount = 0;

					string temp;
					while ((temp = keyStrings[modifiersCount]) != null && TryGetModifierKeys(temp.Trim(), out var currentModifier))
					{
						modifiersCount++;
						modifier |= currentModifier;
					}

					for (int i = modifiersCount; i < keyStrings.Length; i++)
					{
						string keyString = keyStrings[i];
						
						if (keyString == null)
						{
							continue;
						}

						if (KeyConverter.ConvertFrom(keyString.Trim()) is Key key)
						{
							keys.Add(key);
						}
					}

					keySequences.Add(new KeySequence(modifier, keys.ToArray()));
				}

				return new MultiKeyGesture(str, keySequences.ToArray());
			}

			throw GetConvertFromException(value);
		}

		/// <summary>
		///   Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context"> An <see cref="System.ComponentModel.ITypeDescriptorContext" /> that provides a format context. </param>
		/// <param name="culture"> A <see cref="System.Globalization.CultureInfo" /> . If null is passed, the current culture is assumed. </param>
		/// <param name="value"> The <see cref="object" /> to convert. </param>
		/// <param name="destinationType"> The <see cref="System.Type" /> to convert the <paramref name="value" /> parameter to. </param>
		/// <returns> An <see cref="object" /> that represents the converted value. </returns>
		/// <exception cref="System.ArgumentNullException">The
		///   <paramref name="destinationType" />
		///   parameter is null.</exception>
		/// <exception cref="System.NotSupportedException">The conversion cannot be performed.</exception>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType != typeof(string))
			{
				throw GetConvertToException(value, destinationType);
			}

			if (!(value is MultiKeyGesture gesture))
			{
				throw GetConvertToException(value, destinationType);
			}

			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < gesture.KeySequences.Length; i++)
			{
				if (i > 0)
				{
					builder.Append(", ");
				}

				KeySequence sequence = gesture.KeySequences[i];
				if (sequence.Modifiers != ModifierKeys.None)
				{
					builder.Append((string) ModifierKeysConverter.ConvertTo(context, culture, sequence.Modifiers, destinationType));
					builder.Append("+");
				}

				builder.Append((string) KeyConverter.ConvertTo(context, culture, sequence.Keys[0], destinationType));

				for (int j = 1; j < sequence.Keys.Length; j++)
				{
					builder.Append("+");
					builder.Append((string) KeyConverter.ConvertTo(context, culture, sequence.Keys[0], destinationType));
				}
			}

			return builder.ToString();
		}
	}
}