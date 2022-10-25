# MAUI-Azure-Blob_Storage
Test Project for Azure Blob storage access

### Pre-requisits
- you need to setup an [Azure Blob Storage account](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-dotnet-get-started)

### Get started
- clone the repo
- copy the appsettings.sample.json file to appsettings.json
- update "ConnectionString", "ContainerName", and "DocumentContainerName" to the values you have setup in azure

Just here for the actual blob storage code - it's over [here](https://github.com/InquisitorJax/MAUI-Azure-Blob_Storage/blob/main/Storage/CloudFileStorageService.cs)

### TODO
- Add sample code for using a [SAS Token](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-dotnet-get-started#authorize-with-a-sas-token)
- allow for simultaneous uploads amd download progress updates

![image](https://user-images.githubusercontent.com/1822976/197385603-2ddddd6e-6414-4fd4-b69a-1c7b46686a4e.png)
