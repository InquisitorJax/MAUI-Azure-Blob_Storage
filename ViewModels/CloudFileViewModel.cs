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
			System.Diagnostics.Debug.WriteLine($"===================> File Name {CloudFile?.Name}");
		}
	}
}

