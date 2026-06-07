using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace DeltaDNA
{
	internal class ImageMessageStore
	{
		private readonly string cache;

		private readonly MonoBehaviour parent;

		internal ImageMessageStore(MonoBehaviour parent)
		{
		}

		internal virtual bool Has(string url)
		{
			return false;
		}

		internal Texture2D Get(string url)
		{
			return null;
		}

		[DebuggerHidden]
		internal IEnumerator Get(string url, Action<Texture2D> onSuccess, Action<string> onError)
		{
			return null;
		}

		[DebuggerHidden]
		internal IEnumerator Prefetch(Action onSuccess, Action<string> onError, params string[] urls)
		{
			return null;
		}

		internal void Clear()
		{
		}

		[DebuggerHidden]
		private IEnumerator Fetch(string url, Action<Texture2D> onSuccess, Action<string> onError)
		{
			return null;
		}

		private static string GetName(string url)
		{
			return null;
		}
	}
}
