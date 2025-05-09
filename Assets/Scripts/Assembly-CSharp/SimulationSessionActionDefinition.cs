using UnityEngine;

public abstract class SimulationSessionActionDefinition : SessionActionDefinition
{
	protected void FrameCamera(SBCamera camera, Vector2 worldTarget)
	{
		camera.AutoPanToPosition(worldTarget, 0.75f);
	}
}
