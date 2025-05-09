using System.Collections.Generic;

internal class PaperdollSkin
{
	public string name;

	public string skeletonKey;

	public string skeletonReplacement;

	public List<Dictionary<string, string>> propData = new List<Dictionary<string, string>>();

	public Dictionary<string, string> skeletons = new Dictionary<string, string>();
}
