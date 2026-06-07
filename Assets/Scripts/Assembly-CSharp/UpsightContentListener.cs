using System.Collections;
using System.Diagnostics;
using DeltaDNA;
using UnityEngine;

public class UpsightContentListener : Singleton<UpsightContentListener>
{
	[HideInInspector]
	public string _customScope;

	private void Start()
	{
	}

	[DebuggerHidden]
	public IEnumerator _contentCheck()
	{
		return null;
	}
}
