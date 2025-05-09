using UnityEngine;

public class Waypoint
{
	private Simulated sim;

	public Vector2 Position
	{
		get
		{
			return sim.PointOfInterest;
		}
	}

	public Waypoint(Simulated sim)
	{
		this.sim = sim;
	}
}
