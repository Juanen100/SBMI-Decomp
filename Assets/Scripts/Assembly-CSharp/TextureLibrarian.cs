#define ASSERTS_ON
using System.Collections.Generic;
using UnityEngine;

public class TextureLibrarian : MonoBehaviour
{
	private static Dictionary<string, Material> library = new Dictionary<string, Material>();

	public static Material LookUp(string name)
	{
		Material value = null;
		if (!library.TryGetValue(name, out value))
		{
			value = YGTextureLibrary.AtlasMaterial(name);
			if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
			{
				value = Resources.Load(name + "_lr2") as Material;
				if (value == null)
				{
					value = Resources.Load(name + "_lr") as Material;
				}
			}
			else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
			{
				value = Resources.Load(name + "_lr") as Material;
			}
			if (value == null)
			{
				value = Resources.Load(name) as Material;
			}
			library[name] = value;
		}
		TFUtils.Assert(value != null, string.Format("material '{0}' not found", name));
		return value;
	}

	public static string PathLookUp(string name)
	{
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			name += "_lr2";
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			name += "_lr";
		}
		return name;
	}
}
