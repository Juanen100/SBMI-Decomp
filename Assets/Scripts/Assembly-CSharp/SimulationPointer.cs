using System;
using UnityEngine;

public abstract class SimulationPointer : VisualSpawn
{
	private Action simHandler;

	private Action slotHandler;

	protected TerrainSlot slot;

	protected Simulated simulated;

	private BasicSprite sprite;

	private JumpPattern bouncer;

	public virtual Vector3 TargetPosition
	{
		get
		{
			return default(Vector3);
		}
	}

	public void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale)
	{
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale, Simulated parentSimulated)
	{
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale, TerrainSlot slot)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}

	public override void Destroy()
	{
	}
}
