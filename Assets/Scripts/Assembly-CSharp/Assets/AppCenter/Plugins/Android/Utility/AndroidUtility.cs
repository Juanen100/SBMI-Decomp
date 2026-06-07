using UnityEngine;

namespace Assets.AppCenter.Plugins.Android.Utility
{
	internal class AndroidUtility
	{
		private static AndroidJavaObject _context;

		private const string PREFS_NAME = "AppCenterUserPrefs";

		public static AndroidJavaObject GetAndroidContext()
		{
			return null;
		}

		public static void SetPreferenceInt(string prefKey, int prefValue)
		{
		}

		public static void SetPreferenceString(string prefKey, string prefValue)
		{
		}
	}
}
