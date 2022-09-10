using System.Reflection;

namespace AzureBlobFilesApp.Extensions
{
	public static class StringExtensions
	{
		public static string GetStringFromFile(this string file, string resourcePrefix, string assemblyName)
		{
			var assembly = Assembly.Load(new AssemblyName(assemblyName));
			resourcePrefix = resourcePrefix.EndsWith(".") ? resourcePrefix : resourcePrefix + ".";

			Console.WriteLine($"Get {resourcePrefix}{file} as string");

			var stream = assembly.GetManifestResourceStream(resourcePrefix + file);

			if (stream == null)
			{
				Console.WriteLine($"Unable to load chart from app resource: {resourcePrefix}{file}");
				return null;
			}

			using (var reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}
