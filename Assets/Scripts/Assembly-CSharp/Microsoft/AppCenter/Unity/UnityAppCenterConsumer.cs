using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity
{
	public class UnityAppCenterConsumer<T> : AndroidJavaProxy
	{
		internal Action<T> CompletionCallback { get; set; }

		internal UnityAppCenterConsumer()
			: base((string)null)
		{
		}

		private void accept(T t)
		{
		}
	}
}
