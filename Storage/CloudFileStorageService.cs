using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobFilesApp.Extensions;
using Wibci.LogicCommand;

namespace AzureBlobFilesApp.Storage
{
	public interface ICloudFileStorageService
	{
		Task<CloudFileResult> UploadFileAsync(CloudFileType fileType, string fileName, byte[] file);

		Task<CloudFileResult> DownloadFileAsync(CloudFileType fileType, string fileName);

		Task<CloudFileDeleteResult> DeleteFileAsync(CloudFileType fileType, string fileName);

		Task<CloudFilesResult> ListFilesAsync(CloudFileType fileType);
	}

	public class CloudFileStorageService : ICloudFileStorageService
	{
		// doc: https://github.com/Azure-Samples/storage-blobs-xamarin-quickstart/blob/master/src/BlobQuickstartV12/MainPage.xaml.cs

		private BlobServiceClient _blobServiceClient;
		private readonly string CONNECTION_STRING = App.Settings[AppSettings.CONNECTION_STRING];
		private readonly string IMAGE_CONTAINER_NAME = App.Settings[AppSettings.CONTAINER_NAME];
		private readonly string DOCUMENT_CONTAINER_NAME = App.Settings[AppSettings.DOCUMENT_CONTAINER_NAME];

		public CloudFileStorageService()
		{
			_blobServiceClient = new BlobServiceClient(CONNECTION_STRING);
		}

		private BlobClient GetBlobClientForContainer(string containerName, string blobName)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = containerClient.GetBlobClient(blobName);
			return blobClient;
		}

		public async Task<CloudFileResult> UploadFileAsync(CloudFileType fileType, string fileName, byte[] file)
		{
			string containerName = fileType == CloudFileType.Image ? IMAGE_CONTAINER_NAME : DOCUMENT_CONTAINER_NAME;
			var result = await UploadBlobAsync(containerName, fileName, file);
			return result;
		}

		public Task<CloudFileDeleteResult> DeleteFileAsync(CloudFileType fileType, string fileName)
		{
			string containerName = fileType == CloudFileType.Image ? IMAGE_CONTAINER_NAME : DOCUMENT_CONTAINER_NAME;
			return DeleteBlobAsync(containerName, fileName);
		}

		private async Task<CloudFileDeleteResult> DeleteBlobAsync(string containerName, string blobName, BlobClient blobClient = null)
		{
			var result = new CloudFileDeleteResult();

			try
			{
				if (blobClient == null)
					blobClient = GetBlobClientForContainer(containerName, blobName);

				result.Existed = await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);

				if (result.Existed)
				{
					System.Diagnostics.Debug.WriteLine($"===================> Existing copy of {blobName} found and marked for deletion");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"===================> Could not delete blob {blobName} :(");
				result.Fail(ex.Message);
			}

			return result;
		}

		private async Task<CloudFileResult> UploadBlobAsync(string containerName, string blobName, byte[] blob)
		{
			var result = new CloudFileResult();
			System.Diagnostics.Debug.WriteLine($"===================> Uploading blob {blobName}.");

			try
			{
				var blobClient = GetBlobClientForContainer(containerName, blobName);
				
				await DeleteBlobAsync(containerName, blobName, blobClient);

				using (var ms = blob.AsMemoryStream())
				{
					var info = await blobClient.UploadAsync(ms);
					result.File.Url = blobClient.Uri.AbsoluteUri;
					result.File.Name = blobName;
					result.File.Size = blob.Length;
					result.File.Content = blob;
					result.File.LastModified = info.Value.LastModified;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"===================> Could not upload blob :(");
				result.Fail(ex.Message);
			}

			return result;
		}

		public Task<CloudFileResult> DownloadFileAsync(CloudFileType fileType, string fileName)
		{
			string containerName = fileType == CloudFileType.Image ? IMAGE_CONTAINER_NAME : DOCUMENT_CONTAINER_NAME;
			return DownloadBlobAsync(containerName, fileName);
		}

		public Task<CloudFilesResult> ListFilesAsync(CloudFileType fileType)
		{
			string containerName = fileType == CloudFileType.Image ? IMAGE_CONTAINER_NAME : DOCUMENT_CONTAINER_NAME;
			return ListBlobsAsync(containerName, fileType);
		}

		private async Task<CloudFileResult> DownloadBlobAsync(string containerName, string blobName)
		{
			var result = new CloudFileResult();
			System.Diagnostics.Debug.WriteLine($"===================> Downloading blob {blobName}.");

			try
			{
				var blobClient = GetBlobClientForContainer(containerName, blobName);

				BlobDownloadInfo downloadInfo = await blobClient.DownloadAsync();

				using (var memoryStream = new MemoryStream())
				{
					await downloadInfo.Content.CopyToAsync(memoryStream);
					memoryStream.Position = 0;
					result.File = new CloudFile
					{
						Content = memoryStream.ToArray(),
						Name = blobName,
						LastModified = downloadInfo.Details.LastModified,
						Size = memoryStream.Length,
						Url = blobClient.Uri.AbsoluteUri
					};
				
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"===================> Could not download blob {blobName} :(");
				result.Fail(ex.Message);
			}

			return result;
		}

		private async Task<CloudFilesResult> ListBlobsAsync(string containerName, CloudFileType fileType)
		{
			var result = new CloudFilesResult();
			try
			{
				var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

				var blobItems = containerClient.GetBlobsAsync();

                await foreach (var blobItem in blobItems)
				{
					var cloudFile = new CloudFile
					{
						Name = blobItem.Name,
						Size = blobItem.Properties.ContentLength ?? 0,
						FileType = fileType
						//Url = blobItem.ur
					};
					result.Files.Add(cloudFile);
				}
                System.Diagnostics.Debug.WriteLine($"===================> Found {result.Files.Count} files in the {containerName} container");

            }
            catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"===================> Could not list blobs for container {containerName} :(");
				result.Fail(ex.Message);
			}

			return result;
		}
	}

	public class CloudFile
	{
		public string Url { get; set; }

		public string Name { get; set; }

		public double Size { get; set; }

		public byte[] Content { get; set; }

		public DateTimeOffset LastModified { get; set; }

		public CloudFileType FileType { get; set; }
	}

	public enum CloudFileType
	{
		Document,
		Image
	}

	public class CloudFileResult : CommandResult
	{
		public CloudFile File { get; set; } = new CloudFile();
	}

	public class CloudFilesResult : CommandResult
	{
		public List<CloudFile> Files { get; set; } = new List<CloudFile>();
	}

	public class CloudFileDeleteResult : CommandResult
	{
		public bool Existed { get; set; }
	}
}
