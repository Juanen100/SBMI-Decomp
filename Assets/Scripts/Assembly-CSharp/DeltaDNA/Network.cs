using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace DeltaDNA
{
	internal static class Network
	{
		private const string HeaderKey = "STATUS";

		private const string StatusRegex = "^.*\\s(\\d{3})\\s.*$";

		private const string ErrorRegex = "^(\\d{3})\\s.*$";

		[DebuggerHidden]
		internal static IEnumerator SendRequest(HttpRequest request, Action<int, string, string> completionHandler)
		{
			return null;
		}

		private static int ReadStatusCode(WWW www)
		{
			return 0;
		}
	}
}
