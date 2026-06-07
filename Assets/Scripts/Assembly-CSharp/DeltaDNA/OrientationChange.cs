using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace DeltaDNA
{
	public class OrientationChange : MonoBehaviour
	{
		private Vector2 resolution;

		private DeviceOrientation orientation;

		private bool running;

		private event Action onChange
		{
			add
			{
			}
			remove
			{
			}
		}

		private OrientationChange()
		{
		}

		public void Init(Action onChange)
		{
		}

		private void Start()
		{
		}

		[DebuggerHidden]
		private IEnumerator CheckForChange()
		{
			return null;
		}

		private void OnDestroy()
		{
		}
	}
}
