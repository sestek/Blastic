using System.Threading;
using System.Threading.Tasks;

namespace WpfTemplate.Services.Settings
{
	public interface ISettingsService
	{
		Task<bool> Contains(string key, CancellationToken cancellationToken = default);

		Task<string> Get(string key, CancellationToken cancellationToken = default);
		Task Put(string key, string value, CancellationToken cancellationToken = default);

		Task Delete(string key, CancellationToken cancellationToken = default);
	}
}