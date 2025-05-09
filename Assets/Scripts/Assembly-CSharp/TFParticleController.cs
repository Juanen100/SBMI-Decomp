using UnityEngine;

public class TFParticleController
{
	private GameObject gameObject;

	private ParticleSystem particleSystem;

	public Vector3 Position
	{
		get
		{
			return gameObject.transform.position;
		}
		set
		{
			gameObject.transform.position = value;
		}
	}

	public TFParticleController()
	{
		gameObject = UnityGameResources.CreateEmpty("TFParticle");
		particleSystem = gameObject.AddComponent<ParticleSystem>();
		particleSystem.startSize = 20f;
	}

	public void Destroy()
	{
		UnityGameResources.Destroy(gameObject);
	}
}
