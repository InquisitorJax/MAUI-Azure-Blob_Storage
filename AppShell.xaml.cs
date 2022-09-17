using AzureBlobFilesApp.Pages;

namespace AzureBlobFilesApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(CloudFilePage), typeof(CloudFilePage));
	}
}
