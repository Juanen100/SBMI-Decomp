using System.Collections.Generic;
using UnityEngine;

public class Momentum
{
	private int smoothingIterations;

	private List<Vector3> lastPositions = new List<Vector3>();

	private Vector3 velocity = Vector3.zero;

	public Vector3 Velocity
	{
		get
		{
			return velocity;
		}
	}

	public Momentum()
		: this(4)
	{
	}

	public Momentum(int smoothingIterations)
	{
		this.smoothingIterations = smoothingIterations;
	}

	public void Reset()
	{
		velocity = Vector3.zero;
	}

	public void ClearTrackPositions()
	{
		lastPositions.Clear();
	}

	public void TrackForSmoothing(Vector3 position)
	{
		lastPositions.Add(position);
		if (lastPositions.Count > smoothingIterations)
		{
			lastPositions.RemoveAt(0);
		}
	}

	public void CalculateSmoothVelocity()
	{
		velocity = Vector3.zero;
		int num = lastPositions.Count - 1;
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector2 = lastPositions[i + 1] - lastPositions[i];
			vector = vector * 0.1f + vector2 * 0.9f;
		}
		velocity = vector;
	}

	public void ApplyFriction(float amount)
	{
		velocity.Scale(new Vector3(amount, amount, amount));
	}
}
