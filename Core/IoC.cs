namespace AzureBlobFilesApp.Core
{
	public static class IoC
	{
		public static TService Resolve<TService>() => Current.GetService<TService>();

		public static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
			MauiWinUIApplication.Current.Services;
#elif ANDROID
		  MauiApplication.Current.Services;
#elif IOS || MACCATALYST
			MauiUIApplicationDelegate.Current.Services;
#else
			null;
#endif
	}
}
