using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Caliburn.Micro;
using ReactiveUI;

namespace WpfTemplate.Caliburn.Reactive
{
	/// <summary>
	/// A base class that implements the infrastructure for property change notification and automatically performs UI thread marshalling.
	/// </summary>
	[DataContract]
	public class ReactivePropertyChangedBase : ReactiveObject, INotifyPropertyChangedEx
	{
		/// <summary>
		/// Enables/Disables property change notification.
		/// Virtualized in order to help with document oriented view models.
		/// </summary>
		public virtual bool IsNotifying
		{
			get => AreChangeNotificationsEnabled();
			set => throw new NotSupportedException();
		}

		/// <summary>
		/// Raises a change notification indicating that all bindings should be refreshed.
		/// </summary>
		public virtual void Refresh()
		{
			NotifyOfPropertyChange(string.Empty);
		}

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <param name = "propertyName">Name of the property.</param>
		public virtual void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
		{
			this.RaisePropertyChanged(propertyName);
		}

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <typeparam name = "TProperty">The type of the property.</typeparam>
		/// <param name = "property">The property expression.</param>
		public void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
		{
			NotifyOfPropertyChange(ExpressionExtensions.GetMemberInfo(property).Name);
		}

		/// <summary>
		/// Executes the given action on the UI thread
		/// </summary>
		/// <remarks>An extension point for subclasses to customise how property change notifications are handled.</remarks>
		/// <param name="action"></param>
		protected virtual void OnUIThread(System.Action action) => action.OnUIThread();

		/// <summary>
		/// Sets a backing field value and if it's changed raise a notifcation.
		/// </summary>
		/// <typeparam name="T">The type of the value being set.</typeparam>
		/// <param name="oldValue">A reference to the field to update.</param>
		/// <param name="newValue">The new value.</param>
		/// <param name="propertyName">The name of the property for change notifications.</param>
		/// <returns></returns>
		public virtual bool Set<T>(
			ref T oldValue,
			T newValue,
			[CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
			{
				return false;
			}

			oldValue = newValue;

			NotifyOfPropertyChange(propertyName ?? string.Empty);

			return true;
		}
	}
}