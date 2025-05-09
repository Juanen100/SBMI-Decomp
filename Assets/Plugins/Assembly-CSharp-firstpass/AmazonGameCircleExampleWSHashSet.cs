using System;
using System.Collections.Generic;
using UnityEngine;

public class AmazonGameCircleExampleWSHashSet
{
	private const string emptyHashSetLabel = "Key list is empty";

	private const string refreshHashSetButtonLabel = "Refresh";

	private string hashSetTitle;

	private HashSet<string> hashSet;

	private bool foldoutOpen;

	private event Func<HashSet<string>> refreshHashSetFunction;

	public AmazonGameCircleExampleWSHashSet(string title, Func<HashSet<string>> refreshFunction)
	{
		hashSetTitle = title;
		if (refreshFunction != null)
		{
			this.refreshHashSetFunction = (Func<HashSet<string>>)Delegate.Combine(this.refreshHashSetFunction, refreshFunction);
			Refresh();
		}
	}

	public void DrawGUI()
	{
		GUILayout.BeginVertical(GUI.skin.box);
		foldoutOpen = AmazonGameCircleExampleGUIHelpers.FoldoutWithLabel(foldoutOpen, hashSetTitle);
		if (foldoutOpen)
		{
			if (GUILayout.Button("Refresh"))
			{
				Refresh();
			}
			if (hashSet.Count == 0)
			{
				AmazonGameCircleExampleGUIHelpers.CenteredLabel("Key list is empty");
			}
			else
			{
				foreach (string item in hashSet)
				{
					AmazonGameCircleExampleGUIHelpers.CenteredLabel(item);
				}
			}
		}
		GUILayout.EndVertical();
	}

	public void Refresh()
	{
		if (this.refreshHashSetFunction != null)
		{
			hashSet = this.refreshHashSetFunction();
		}
	}
}
