using AzureBlobFilesApp.Extensions;
using System.Text.Json;

namespace AzureBlobFilesApp
{
	public class AppSettings
	{
		public const string CONNECTION_STRING = "ConnectionString";
		public const string CONTAINER_NAME = "ContainerName";
		public const string DOCUMENT_CONTAINER_NAME = "DocumentContainerName";

		private const string NAMESPACE = "AzureBlobFilesApp";

        private const string FILE_NAME = "appsettings.json";

		private readonly Dictionary<string, string> _secrets;

		public AppSettings()
		{
			var fileString = FILE_NAME.GetStringFromFile(NAMESPACE, typeof(AppSettings).Assembly.FullName);
			_secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(fileString);
		}

		public string this[string name]
		{
			get
			{
				try
				{
					if (_secrets.TryGetValue(name, out string configValue))
					{
						return configValue;
					}

					Console.WriteLine($"Unable to retrieve secret '{name}'");
					return "ERROR";
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Unable to retrieve secret '{name}'");
					return string.Empty;
				}
			}
		}
	}
}
