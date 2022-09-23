using AzureBlobFilesApp.Extensions;
using AzureBlobFilesApp.Pages;
using AzureBlobFilesApp.Storage;
using Bumptech.Glide.Signature;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AzureBlobFilesApp.ViewModels
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
				await LoadFilesAsync();
			}
			finally
			{
				BusyMessage = String.Empty;
				IsBusy = false;
			}
		}

		private async Task LoadFilesAsync()
		{
			await LoadImagesAsync();
			await LoadDocumentsAsync();
		}

		[RelayCommand]
		private async Task LoadImagesAsync()
		{
			var images = new ObservableCollection<CloudFile>();
			var imagesResult = await _storageService.ListFilesAsync(CloudFileType.Image);
			if (imagesResult.IsValid())
			{
				foreach (var file in imagesResult.Files)
				{
					images.Add(file);
				}
			}
			Images = images;
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

		[RelayCommand]
		private async Task DeleteCloudFileAsync(CloudFile file)
		{
			IsBusy = true;
			BusyMessage = $"Deleting {file.Name}";
			try
			{
				var deleteResult =  await _storageService.DeleteFileAsync(file.FileType, file.Name);
				if (deleteResult.IsValid())
				{
					await LoadFilesAsync();
				}
            }
            finally
			{
				IsBusy = false;
				BusyMessage = String.Empty;
			}
		}

		[RelayCommand]
		private async Task OpenFileAsync(CloudFile file)
		{
			await Shell.Current.GoToAsync($"{nameof(CloudFilePage)}", 
				new Dictionary<string, object>
				{
					[nameof(CloudFile)] = file 
				});
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

				// refresh the image list (duplicate file name could have been used)
				await LoadImagesAsync();
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

				// refresh the document list (duplicate file name could have been used)
				await LoadDocumentsAsync();
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
