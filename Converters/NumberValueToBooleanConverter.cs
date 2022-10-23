using System.Globalization;

namespace AzureBlobFilesApp.Converters
{
	internal class NumberValueToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool retValue = true;

			if (value != null && value is long numberValue && parameter != null)
			{
				long compareValue = 0;
				long.TryParse(parameter.ToString(), out compareValue);
				retValue = compareValue != numberValue;
			}

			return retValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
