using System.Collections.Generic;
using UnityEngine;

public class TextureLibrarian : MonoBehaviour
{
	private static Dictionary<string, Material> library;

	public bool TextureIsLoaded(string name)
	{
		return false;
	}

	public static Material LookUp(string name)
	{
		return null;
	}

	private static string Path(string path)
	{
		return null;
	}

	public static Material LookUp(string name, string path, bool ignoreAtlas = false)
	{
		return null;
	}

	public static string PathLookUp(string name)
	{
		return null;
	}
}
