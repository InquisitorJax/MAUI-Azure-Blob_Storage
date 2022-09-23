using AzureBlobFilesApp.Storage;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AzureBlobFilesApp.ViewModels
{
	[QueryProperty(nameof(CloudFile), nameof(CloudFile))] // populated via object dictionary in navigation
	[ObservableObject]
	public partial class CloudFileViewModel : IQueryAttributable
	{
		public CloudFileViewModel()
		{
		}

		[ObservableProperty]
		private CloudFile _cloudFile;

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			// no need to check query parameters here, is CloudFile gets populated by QueryProperty after this method
		}

		public async Task InitializeAsync()
		{
			System.Diagnostics.Debug.WriteLine($"===================> File Name {CloudFile?.Name}");
		}
	}
}

