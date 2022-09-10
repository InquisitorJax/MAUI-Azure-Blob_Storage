namespace AzureBlobFilesApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		Settings = new AppSettings();

		MainPage = new AppShell();
	}

	public static AppSettings Settings { get; private set; }
}
