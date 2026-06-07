using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Internal.Utility
{
	public class JavaDateHelper
	{
		private const string DotNetDateFormat = "yyyy-MM-dd'T'HH:mm:ss.fffK";

		private static AndroidJavaObject _javaDateFormatter;

		private static AndroidJavaObject JavaDateFormatter
		{
			get
			{
				return null;
			}
		}

		public static AndroidJavaObject DateTimeConvert(DateTime date)
		{
			return null;
		}

		public static DateTimeOffset DateTimeConvert(AndroidJavaObject date)
		{
			return default(DateTimeOffset);
		}
	}
}
