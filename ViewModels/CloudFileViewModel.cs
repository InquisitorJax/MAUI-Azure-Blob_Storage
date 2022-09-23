using AzureBlobFilesApp.Storage;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AzureBlobFilesApp.ViewModels
{
	[QueryProperty(nameof(CloudFile), nameof(CloudFile))] // populated via object dictionary in navigation
	[ObservableObject]
	public partial class CloudFileViewModel : IQueryAttributable
	{
		private readonly ICloudFileStorageService _storageService;

		public CloudFileViewModel(ICloudFileStorageService storageService)
		{
			_storageService = storageService;
		}

		[ObservableProperty]
		private CloudFile _cloudFile;

		[ObservableProperty]
		private bool _isBusy;

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			// no need to check query parameters here, is CloudFile gets populated by QueryProperty after this method
		}

		public async Task InitializeAsync()
		{
			System.Diagnostics.Debug.WriteLine($"===================> File Name {CloudFile?.Name}");
			if (CloudFile != null)
			{
				IsBusy = true;

				try
				{
					// let's download the file to get the .Content property populated
					var downloadedResult = await _storageService.DownloadFileAsync(CloudFile.FileType, CloudFile.Name);
					if (downloadedResult.IsValid())
					{
						CloudFile = downloadedResult.File;
					}
				}
				finally
				{
					IsBusy = false;
				}
			}
		}
	}
}

