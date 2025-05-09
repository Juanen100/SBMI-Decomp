using System.Collections.Generic;
using UnityEngine;

public class UnityGameResources
{
	private static List<GameObject> gameObjects;

	static UnityGameResources()
	{
		gameObjects = new List<GameObject>();
	}

	public static GameObject CreateEmpty(string objectName)
	{
		GameObject gameObject = new GameObject(objectName);
		gameObjects.Add(gameObject);
		return gameObject;
	}

	public static GameObject Create(string localPath)
	{
		GameObject gameObject = Object.Instantiate(Resources.Load(localPath)) as GameObject;
		gameObjects.Add(gameObject);
		return gameObject;
	}

	public static GameObject SafeCreate(string localPath)
	{
		Object obj = Resources.Load(localPath);
		if (obj == null)
		{
			return null;
		}
		GameObject gameObject = Object.Instantiate(obj) as GameObject;
		gameObjects.Add(gameObject);
		return gameObject;
	}

	public static void AddGameObject(GameObject obj)
	{
		gameObjects.Add(obj);
	}

	public static void Destroy(GameObject gameObject)
	{
		gameObjects.Remove(gameObject);
		Object.Destroy(gameObject);
	}

	public static void Reset()
	{
		foreach (GameObject gameObject in gameObjects)
		{
			Object.Destroy(gameObject);
		}
		gameObjects.Clear();
	}
}
