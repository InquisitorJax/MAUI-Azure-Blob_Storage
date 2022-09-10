using System.Reflection;

namespace AzureBlobFilesApp.Extensions
{
	internal static class ResourceFileExtensions
	{
		public static byte[] LoadAppResourceFromFile(this string resourceFileName, string customPrefix = null)
		{
			var assembly = typeof(App).GetTypeInfo().Assembly; //alt: var assembly = Assembly.Load(new AssemblyName("WibciLabs.Remotime"));
			customPrefix = string.IsNullOrEmpty(customPrefix) ? "AzureBlobFilesApp.Resources.Files." : customPrefix;
			string fileName = customPrefix + resourceFileName;

			byte[] buffer;
			using (Stream s = assembly.GetManifestResourceStream(fileName))
			{
				long length = s.Length;
				buffer = new byte[length];
				s.Read(buffer, 0, (int)length);
			}

			return buffer;
		}

		public static T GetAppResource<T>(string resourceName)
		{
			//NOTE: Must use TryGetValue with out var in order to search within merged dictionaries as well
			Application.Current.Resources.TryGetValue(resourceName, out var retValue);
			return (T)retValue;
		}

		public static Color GetResourceColor(string colorName)
		{
			var appThemeColor = GetAppResource<Color>(colorName);
			return appThemeColor;
		}
	}
}
