using AzureBlobFilesApp.Pages;
using AzureBlobFilesApp.Storage;
using AzureBlobFilesApp.ViewModels;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Syncfusion.Maui.Core.Hosting;

namespace AzureBlobFilesApp.Core
{
	public static class ServiceExtensions
	{
		public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
		{
			builder.Services.TryAddTransient<ICloudFileStorageService, CloudFileStorageService>();

			return builder;
		}

		public static MauiAppBuilder ConfigureMvvm(this MauiAppBuilder builder)
		{
			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddTransient<MainViewModel>();

			builder.Services.AddTransient<CloudFilePage>();
			builder.Services.AddTransient<CloudFileViewModel>();

			return builder;
		}


		public static MauiAppBuilder ConfigureSyncfusion(this MauiAppBuilder builder)
		{
			builder.ConfigureSyncfusionCore();

			return builder;
		}

	}
}
