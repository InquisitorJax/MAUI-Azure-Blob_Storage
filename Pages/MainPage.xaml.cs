using AzureBlobFilesApp.ViewModels;

namespace AzureBlobFilesApp.Pages;

public partial class MainPage : ContentPage
{

	private MainViewModel _viewModel;
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();

		//var viewModel = IoC.Resolve<MainPageViewModel>();
		BindingContext = _viewModel = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Task.Run(async () =>
		{
			await _viewModel.InitializeAsync();
		});
	}
}

