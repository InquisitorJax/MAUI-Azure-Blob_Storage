using AzureBlobFilesApp.ViewModels;

namespace AzureBlobFilesApp.Pages;

public partial class CloudFilePage : ContentPage
{
	private CloudFileViewModel _viewModel;

	public CloudFilePage(CloudFileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = _viewModel = viewModel;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		// better than using OnAppearing - which fires on different contexts esp. on Android (like show modals or foregrounding app)
		base.OnNavigatedTo(args);
		await _viewModel.InitializeAsync();
	}
}