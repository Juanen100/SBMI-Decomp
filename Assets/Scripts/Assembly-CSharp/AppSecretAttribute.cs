using UnityEngine;

public class AppSecretAttribute : PropertyAttribute
{
	public string Name { get; set; }

	public AppSecretAttribute()
	{
	}

	public AppSecretAttribute(string name)
	{
	}
}
