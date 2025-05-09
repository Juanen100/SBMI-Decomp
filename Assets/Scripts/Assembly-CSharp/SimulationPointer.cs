#define ASSERTS_ON
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
			return offset;
		}
	}

	public void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale)
	{
		base.Initialize(game, action, offset, 0f, alpha, scale);
		bouncer = new JumpPattern(-200f, 12f, 0.25f, 0.18f, 0f, Time.time, Vector2.one);
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Standard)
		{
			sprite = new BasicSprite("Materials/lod/TutorialPointer_lr", null, Vector2.zero, 17f, 29f);
		}
		else if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			sprite = new BasicSprite("Materials/lod/TutorialPointer_lr2", null, Vector2.zero, 17f, 29f);
		}
		else
		{
			sprite = new BasicSprite("Materials/lod/TutorialPointer", null, Vector2.zero, 17f, 29f);
		}
		sprite.PublicInitialize();
		sprite.Billboard(SBCamera.BillboardDefinition);
		TFUtils.Assert(base.Alpha == 1f, "Simulated Pointers do not support alpha blending yet. Needs to be implemented");
		NormalizeRotationAndPushToEdge(0f, 0f);
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale, Simulated parentSimulated)
	{
		simulated = parentSimulated;
		if (simulated != null)
		{
			simHandler = delegate
			{
				if (action.Status != SessionActionTracker.StatusCode.OBLITERATED)
				{
					action.MarkSucceeded();
				}
			};
			simulated.AddClickListener(simHandler);
		}
		Initialize(game, action, offset, alpha, scale);
	}

	protected void Initialize(Game game, SessionActionTracker action, Vector3 offset, float alpha, Vector2 scale, TerrainSlot slot)
	{
		this.slot = slot;
		if (this.slot != null)
		{
			slotHandler = delegate
			{
				if (action.Status != SessionActionTracker.StatusCode.OBLITERATED)
				{
					action.MarkSucceeded();
				}
			};
			this.slot.AddClickListener(slotHandler);
		}
		Initialize(game, action, offset, alpha, scale);
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		float val;
		Vector2 squish;
		bouncer.ValueAndSquishAtTime(Time.time, out val, out squish);
		if (!TFPerfUtils.IsNonScalingDevice())
		{
			sprite.Scale = TFUtils.ExpandVector(squish);
		}
		Vector3 vector = new Vector3(0f, 0f, val + sprite.Height * sprite.Scale.y);
		sprite.Position = TargetPosition + vector;
		sprite.OnUpdate(game.simulation.TheCamera, null);
		if (simulated != null && !simulated.Visible)
		{
			base.ParentAction.MarkFailed();
		}
		return base.OnUpdate(game);
	}

	public override void Destroy()
	{
		if (simulated != null)
		{
			simulated.RemoveClickListener(simHandler);
		}
		if (slot != null)
		{
			slot.RemoveClickListener(slotHandler);
		}
		if (sprite != null)
		{
			sprite.Destroy();
		}
	}
}
