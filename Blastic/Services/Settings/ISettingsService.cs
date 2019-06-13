using System.Threading;
using System.Threading.Tasks;

namespace Blastic.Services.Settings
{
	public interface ISettingsService
	{
		Task<bool> Contains(string key, CancellationToken cancellationToken = default);

		Task<T> Get<T>(string key, T defaultValue = default, CancellationToken cancellationToken = default);
		Task Put<T>(string key, T value, CancellationToken cancellationToken = default);

		Task Delete(string key, CancellationToken cancellationToken = default);
	}
}