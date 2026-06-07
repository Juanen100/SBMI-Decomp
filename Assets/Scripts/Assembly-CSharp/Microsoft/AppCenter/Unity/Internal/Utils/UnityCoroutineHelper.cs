using System;
using System.Collections;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Internal.Utils
{
	public class UnityCoroutineHelper : MonoBehaviour
	{
		private static UnityCoroutineHelper Instance
		{
			get
			{
				return null;
			}
		}

		public static void StartCoroutine(Func<IEnumerator> coroutine)
		{
		}
	}
}
