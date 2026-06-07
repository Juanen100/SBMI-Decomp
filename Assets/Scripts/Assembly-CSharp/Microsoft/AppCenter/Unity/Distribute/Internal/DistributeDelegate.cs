using UnityEngine;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
	internal class DistributeDelegate : AndroidJavaProxy
	{
		private DistributeDelegate()
			: base((string)null)
		{
		}

		public static void SetDelegate()
		{
		}

		private bool onReleaseAvailable(AndroidJavaObject activity, AndroidJavaObject details)
		{
			return false;
		}
	}
}
