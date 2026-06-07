namespace Microsoft.AppCenter.Unity.Crashes.Models
{
	public class Exception
	{
		public string Message { get; private set; }

		public string StackTrace { get; private set; }

		public Exception(string message, string stackTrace)
		{
		}
	}
}
