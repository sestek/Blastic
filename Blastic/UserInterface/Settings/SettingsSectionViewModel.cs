using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blastic.Caliburn;
using Blastic.Diagnostics;
using Blastic.Execution;

namespace Blastic.UserInterface.Settings
{
	public abstract class SettingsSectionViewModel : ScreenBase, ISettingsSectionViewModel, IDataErrorInfo
	{
		private Dictionary<string, SettingInfo> _settings;

		public abstract string SectionName { get; }

		protected SettingsSectionViewModel(ExecutionContextFactory executionContextFactory) : base(executionContextFactory)
		{
		}

		protected override Task OnInitializeAsync(CancellationToken cancellationToken)
		{
			_settings = GetType()
				.GetProperties()
				.Where(x => IsAssignableToGenericType(x.PropertyType, typeof(Setting<>)))
				.ToDictionary(
					x => x.Name,
					x => new SettingInfo(x, x.GetValue(this)));

			return Task.CompletedTask;
		}

		public async Task Save(CancellationToken cancellationToken)
		{
			foreach (SettingInfo info in _settings.Values)
			{
				await (Task)info.SaveMethod.Invoke(info.Setting, new object[]{ cancellationToken });
			}
		}

		public async Task ReadSettings(CancellationToken cancellationToken)
		{
			foreach (SettingInfo info in _settings.Values)
			{
				await (Task)info.ReadMethod.Invoke(info.Setting, new object[] { cancellationToken });
			}
		}

		public void Revert()
		{
			foreach (SettingInfo info in _settings.Values)
			{
				info.RevertMethod.Invoke(info.Setting, Array.Empty<object>());
			}
		}

		public virtual Task<IEnumerable<DiagnosticMessage>> GetDiagnosticMessages(CancellationToken cancellationToken)
		{
			return Task.FromResult(Enumerable.Empty<DiagnosticMessage>());
		}

		public string this[string columnName]
		{
			get
			{
				if (_settings.TryGetValue(columnName, out SettingInfo info))
				{
					return (string)info.CheckErrorMethod.Invoke(info.Setting, Array.Empty<object>());
				}

				return null;
			}
		}

		public string Error => null;

		private static bool IsAssignableToGenericType(Type givenType, Type genericType)
		{
			Type[] interfaceTypes = givenType.GetInterfaces();

			if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
			{
				return true;
			}

			foreach (Type it in interfaceTypes)
			{
				if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
			}

			Type baseType = givenType.BaseType;
			
			if (baseType == null)
			{
				return false;
			}

			return IsAssignableToGenericType(baseType, genericType);
		}

		private class SettingInfo
		{
			public object Setting { get; }

			public MethodInfo SaveMethod { get; }
			public MethodInfo ReadMethod { get; }
			public MethodInfo RevertMethod { get; }
			public MethodInfo CheckErrorMethod { get; }

			public SettingInfo(PropertyInfo propertyInfo, object setting)
			{
				Setting = setting;

				SaveMethod = GetMethodInfo(nameof(Setting<object>.Save), propertyInfo.PropertyType);
				ReadMethod = GetMethodInfo(nameof(Setting<object>.Read), propertyInfo.PropertyType);
				RevertMethod = GetMethodInfo(nameof(Setting<object>.Revert), propertyInfo.PropertyType);
				CheckErrorMethod = GetMethodInfo(nameof(Setting<object>.CheckError), propertyInfo.PropertyType);
			}

			private MethodInfo GetMethodInfo(
				string methodName,
				Type propertyType)
			{
				MethodInfo methodInfo = propertyType.GetMethod(methodName);

				if (methodInfo == null)
				{
					throw new InvalidOperationException($"{methodName} method is not found on {propertyType}!");
				}

				return methodInfo;
			}
		}
	}
}