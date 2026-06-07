using System;
using System.Collections.Generic;

public class AmazonGameCircleExampleWSHashSet
{
	private string hashSetTitle;

	private HashSet<string> hashSet;

	private bool foldoutOpen;

	private const string emptyHashSetLabel = "Key list is empty";

	private const string refreshHashSetButtonLabel = "Refresh";

	private event Func<HashSet<string>> refreshHashSetFunction
	{
		add
		{
		}
		remove
		{
		}
	}

	public AmazonGameCircleExampleWSHashSet(string title, Func<HashSet<string>> refreshFunction)
	{
	}

	public void DrawGUI()
	{
	}

	public void Refresh()
	{
	}
}
