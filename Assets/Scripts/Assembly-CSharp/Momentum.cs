using System.Collections.Generic;
using UnityEngine;

public class Momentum
{
	private int smoothingIterations;

	private List<Vector3> lastPositions;

	private Vector3 velocity;

	public Vector3 Velocity
	{
		get
		{
			return default(Vector3);
		}
	}

	public Momentum()
	{
	}

	public Momentum(int smoothingIterations)
	{
	}

	public void Reset()
	{
	}

	public void ClearTrackPositions()
	{
	}

	public void TrackForSmoothing(Vector3 position)
	{
	}

	public void CalculateSmoothVelocity()
	{
	}

	public void ApplyFriction(float amount)
	{
	}
}
