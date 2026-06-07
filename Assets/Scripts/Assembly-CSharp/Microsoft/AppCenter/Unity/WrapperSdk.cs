namespace Microsoft.AppCenter.Unity
{
	public static class WrapperSdk
	{
		private static string _wrapperRuntimeVersion;

		private static bool _hasAttemptedToGetRuntimeVersion;

		public const string Name = "appcenter.unity";

		public const string WrapperSdkVersion = "2.3.0";

		internal static string WrapperRuntimeVersion
		{
			get
			{
				return null;
			}
		}

		private static string GetWrapperRuntimeVersion()
		{
			return null;
		}
	}
}
