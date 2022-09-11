using AzureBlobFilesApp.Extensions;
using AzureBlobFilesApp.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AzureBlobFilesApp
{
	[ObservableObject]
	public partial class MainViewModel
	{
		private readonly ICloudFileStorageService _storageService;
		private const string ImageResourceName = "IndieDads.png";
		private const string DocumentResourceName = "Mapungubwe.pdf";
		private byte[] _imageResource;
		private byte[] _documentResource;

		public MainViewModel(ICloudFileStorageService storageService)
		{
			_storageService = storageService;
			_images = new ObservableCollection<CloudFile>();
			_documents = new ObservableCollection<CloudFile>();
		}

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private int _selectedIndex;

		[ObservableProperty]
		private ObservableCollection<CloudFile> _images;

		[ObservableProperty]
		private ObservableCollection<CloudFile> _documents;

		[ObservableProperty]
		private string _imageErrorMessage;

		[ObservableProperty]
		private string _documentErrorMessage;

		[ObservableProperty]
		private string _busyMessage;

		public async Task InitializeAsync()
		{
			IsBusy = true;
			BusyMessage = "Fetching all cloud files...";

			try
			{
				await LoadImagesAsync();
				await LoadDocumentsAsync();
			}
			finally
			{
				BusyMessage = String.Empty;
				IsBusy = false;
			}
		}

		[RelayCommand]
		private async Task LoadImagesAsync()
		{
			Images = new ObservableCollection<CloudFile>();
			var imagesResult = await _storageService.ListFilesAsync(CloudFileType.Image);
			if (imagesResult.IsValid())
			{
				foreach (var file in imagesResult.Files)
				{
					Images.Add(file);
				}
			}
		}

		[RelayCommand]
		private async Task LoadDocumentsAsync()
		{
			Documents = new ObservableCollection<CloudFile>();
			var documentsResult = await _storageService.ListFilesAsync(CloudFileType.Document);
			if (documentsResult.IsValid())
			{
				foreach (var document in documentsResult.Files)
				{
					Documents.Add(document);
				}
			}
		}

		[RelayCommand]
		private async Task AddCloudFileAsync()
		{
			switch (SelectedIndex)
			{
				case 0:
					await AddImageAsync();
					break;
				default:
					await AddDocumentAsync();
					break;
			}
		}

		private async Task AddImageAsync()
		{
			IsBusy = true;
			BusyMessage = "Adding Image...";

			try
			{
				string fileName = ImageResourceName.Replace(".png", $"{Images.Count + 1}.png");
				ImageErrorMessage = string.Empty;

				LoadImageResource();

				var uploadResult = await _storageService.UploadFileAsync(CloudFileType.Image, fileName, _imageResource);

				if (uploadResult.IsValid())
				{
					Images.Add(uploadResult.File);
				}
				else
				{
					ImageErrorMessage = uploadResult.ToString();
				}
			}
			finally
			{
				BusyMessage = String.Empty;
				IsBusy = false;
			}

		}

		private async Task AddDocumentAsync()
		{
			IsBusy = true;
			BusyMessage = "Adding Document...";

			try
			{
				string fileName = DocumentResourceName.Replace(".pdf", $"{Documents.Count + 1}.pdf");
				ImageErrorMessage = string.Empty;

				LoadDocumentResource();

				var uploadResult = await _storageService.UploadFileAsync(CloudFileType.Document, fileName, _documentResource);

				if (uploadResult.IsValid())
				{
					Documents.Add(uploadResult.File);
				}
				else
				{
					DocumentErrorMessage = uploadResult.ToString();
				}
			}
			finally
			{
				BusyMessage = String.Empty;
				IsBusy = false;
			}
		
		}

		private byte[] LoadImageResource()
		{
			if (_imageResource == null)
			{
				_imageResource = ImageResourceName.LoadAppResourceFromFile();
			}

			return _imageResource;
		}

		private byte[] LoadDocumentResource()
		{
			if (_documentResource == null)
			{
				_documentResource = DocumentResourceName.LoadAppResourceFromFile();
			}
			return _documentResource;
		}

	}
}
