using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes
{
	public class WrapperException
	{
		private readonly AndroidJavaObject _rawObject;

		internal AndroidJavaObject GetRawObject()
		{
			return null;
		}

		public void SetType(string type)
		{
		}

		public void SetMessage(string message)
		{
		}

		public void SetStacktrace(string stacktrace)
		{
		}

		public void SetInnerException(AndroidJavaObject innerException)
		{
		}

		public void SetWrapperSdkName(string sdkName)
		{
		}
	}
}
