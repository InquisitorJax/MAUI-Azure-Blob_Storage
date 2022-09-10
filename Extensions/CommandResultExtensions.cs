using Wibci.LogicCommand;

namespace AzureBlobFilesApp.Extensions
{
	public static class CommandResultExtensions
	{
		public static void Fail(this CommandResult result, string message)
		{
			if (result != null)
			{
				result.Notification.Fail(message);
			}
		}
	}

	public static class NotificationExtensions
	{
		public static void Fail(this Notification notification, string message)
		{
			if (notification != null)
			{
				notification.Add(new NotificationItem(message));
			}
		}
	}

}
