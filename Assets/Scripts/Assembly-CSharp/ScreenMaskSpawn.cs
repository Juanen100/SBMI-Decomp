using System;
using UnityEngine;

public class ScreenMaskSpawn : SessionActionSpawn
{
	[Flags]
	public enum ScreenMaskType
	{
		ELEMENT = 0,
		SIMULATED = 1,
		SIMULATION = 2,
		EXPANSION = 3
	}

	private ScreenMaskType maskType;

	private SBGUIElement uiElement;

	private Simulated simulated;

	private TerrainSlot slot;

	private Action simHandler;

	private Action slotHandler;

	private GameObject screenMaskGO;

	private GameObject screenMaskGO2;

	private Camera camera2;

	private bool fullScreen;

	private bool fullScreen2;

	private float screenZ;

	private Vector3 offset;

	private float offsetAbsMax;

	private float borderStepX;

	private float borderStepY;

	private float centerStepX;

	private float centerStepY;

	private UiSpawnMixin uiMixin;

	protected Simulation simulation;

	private ScreenMaskSpawn(ScreenMaskType maskType, Game game, SessionActionTracker parentAction)
	{
	}

	public static void Spawn(ScreenMaskType maskType, Game game, SessionActionTracker parentAction, SBGUIElement parentElement, SBGUIScreen containingScreen, Simulated parentSimulated, TerrainSlot slot, float radius, string texture, Vector3 offset, bool useSecondCam = false)
	{
	}

	protected void RegisterNewInstanceForElement(Game game, SessionActionTracker parentAction, SBGUIElement uiElement, SBGUIScreen containingScreen, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
	}

	private Camera GetSecondUICamera()
	{
		return null;
	}

	protected void RegisterNewInstanceForSimulated(Game game, SessionActionTracker parentAction, Simulated parentSimulated, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
	}

	protected void RegisterNewInstanceForExpansion(Game game, SessionActionTracker parentAction, TerrainSlot slot, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		return default(SessionActionManager.SpawnReturnCode);
	}

	public override void Destroy()
	{
	}

	private void CreateScreenMaskMesh(float radius, string texture, Vector3 offset, Camera secondCam, bool coverFullScreen = false)
	{
	}

	private void UpdateDynamicElement(Camera cam, out float offsetX, out float offsetY)
	{
		offsetX = default(float);
		offsetY = default(float);
	}

	private void ClampOffset(Camera cam, ref float offsetX, ref float offsetY)
	{
	}
}
