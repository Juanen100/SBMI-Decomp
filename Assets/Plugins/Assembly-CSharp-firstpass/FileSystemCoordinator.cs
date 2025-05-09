using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileSystemCoordinator
{
	private const string BundlePaths = "/Contents/Bundles/";

	private static Dictionary<string, UnityEngine.Object> sFilePaths = new Dictionary<string, UnityEngine.Object>();

	public static void Clear()
	{
		sFilePaths = new Dictionary<string, UnityEngine.Object>();
	}

	public static UnityEngine.Object LoadAsset(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return null;
		}
		UnityEngine.Object value = null;
		try
		{
			if (!sFilePaths.TryGetValue(fileName, out value))
			{
				string path = Application.persistentDataPath + "/Contents/Bundles/" + fileName + "_Android.bundle";
				if (File.Exists(path))
				{
					BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
					AssetBundle assetBundle = AssetBundle.CreateFromMemoryImmediate(binaryReader.ReadBytes((int)binaryReader.BaseStream.Length));
					binaryReader.Close();
					binaryReader = null;
					if (assetBundle == null)
					{
						Debug.Log("FileSystemCoordinator: No Bundle Loaded: " + fileName);
					}
					FileSystemBundle component = ((GameObject)assetBundle.mainAsset).GetComponent<FileSystemBundle>();
					if (component == null)
					{
						Debug.Log("FileSystemCoordinator: Invalid Main Game Object: " + fileName);
					}
					int num = component.fileObjects.Length;
					for (int i = 0; i < num; i++)
					{
						sFilePaths.Add(component.filePaths[i], component.fileObjects[i]);
						if (component.filePaths[i] == fileName)
						{
							value = component.fileObjects[i];
						}
					}
					assetBundle.Unload(false);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);
		}
		return value;
	}
}
