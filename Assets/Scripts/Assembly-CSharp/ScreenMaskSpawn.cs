#define ASSERTS_ON
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

	private float screenZ = 0.2f;

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
		this.maskType = maskType;
		simulation = game.simulation;
		base.RegisterNewInstance(game, parentAction);
	}

	public static void Spawn(ScreenMaskType maskType, Game game, SessionActionTracker parentAction, SBGUIElement parentElement, SBGUIScreen containingScreen, Simulated parentSimulated, TerrainSlot slot, float radius, string texture, Vector3 offset, bool useSecondCam = false)
	{
		ScreenMaskSpawn screenMaskSpawn = new ScreenMaskSpawn(maskType, game, parentAction);
		switch (maskType)
		{
		case ScreenMaskType.ELEMENT:
			screenMaskSpawn.RegisterNewInstanceForElement(game, parentAction, parentElement, containingScreen, radius, texture, offset, useSecondCam);
			break;
		case ScreenMaskType.EXPANSION:
			screenMaskSpawn.RegisterNewInstanceForExpansion(game, parentAction, slot, radius, texture, offset, useSecondCam);
			break;
		default:
			screenMaskSpawn.RegisterNewInstanceForSimulated(game, parentAction, parentSimulated, radius, texture, offset, useSecondCam);
			break;
		}
	}

	protected void RegisterNewInstanceForElement(Game game, SessionActionTracker parentAction, SBGUIElement uiElement, SBGUIScreen containingScreen, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
		this.uiElement = uiElement;
		uiMixin = new UiSpawnMixin();
		uiMixin.OnRegisterNewInstance(parentAction, containingScreen);
		if (!parentAction.ManualSuccess)
		{
			SBGUIButton[] componentsInChildren = this.uiElement.GetComponentsInChildren<SBGUIButton>();
			foreach (SBGUIButton sBGUIButton in componentsInChildren)
			{
				sBGUIButton.ClickEvent += delegate
				{
					parentAction.MarkSucceeded(false);
				};
			}
		}
		if (maskType == ScreenMaskType.ELEMENT)
		{
			camera2 = GetSecondUICamera();
			if ((bool)camera2)
			{
				if (useSecondCam)
				{
					CreateScreenMaskMesh(radius, null, offset, null, true);
					CreateScreenMaskMesh(radius, texture, offset, camera2);
					fullScreen = true;
				}
				else
				{
					CreateScreenMaskMesh(radius, texture, offset, null);
					CreateScreenMaskMesh(radius, null, offset, camera2, true);
					fullScreen2 = true;
				}
			}
		}
		if (camera2 == null)
		{
			CreateScreenMaskMesh(radius, texture, offset, null);
		}
	}

	private Camera GetSecondUICamera()
	{
		Camera camera = GUIMainView.GetInstance().GetComponent<Camera>();
		GameObject gameObject = camera.gameObject;
		Camera[] componentsInChildren = gameObject.GetComponentsInChildren<Camera>();
		Camera[] array = componentsInChildren;
		foreach (Camera camera2 in array)
		{
			if (camera2 != camera)
			{
				return camera2;
			}
		}
		return null;
	}

	protected void RegisterNewInstanceForSimulated(Game game, SessionActionTracker parentAction, Simulated parentSimulated, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
		simulated = parentSimulated;
		if (simulated != null)
		{
			simHandler = delegate
			{
				if (parentAction.Status == SessionActionTracker.StatusCode.STARTED)
				{
					parentAction.MarkSucceeded();
				}
			};
			simulated.AddClickListener(simHandler);
		}
		CreateScreenMaskMesh(radius, texture, offset, null);
	}

	protected void RegisterNewInstanceForExpansion(Game game, SessionActionTracker parentAction, TerrainSlot slot, float radius, string texture, Vector3 offset, bool useSecondCam)
	{
		this.slot = slot;
		if (this.slot != null)
		{
			slotHandler = delegate
			{
				if (parentAction.Status == SessionActionTracker.StatusCode.STARTED)
				{
					parentAction.MarkSucceeded();
				}
			};
			this.slot.AddClickListener(slotHandler);
		}
		CreateScreenMaskMesh(radius, texture, offset, null);
	}

	public override SessionActionManager.SpawnReturnCode OnUpdate(Game game)
	{
		SessionActionManager.SpawnReturnCode spawnReturnCode = SessionActionManager.SpawnReturnCode.KEEP_ALIVE;
		if (maskType == ScreenMaskType.ELEMENT)
		{
			spawnReturnCode = base.OnUpdate(game);
			if (spawnReturnCode != SessionActionManager.SpawnReturnCode.KILL)
			{
				if (screenMaskGO != null && !fullScreen)
				{
					float offsetX;
					float offsetY;
					UpdateDynamicElement(GUIMainView.GetInstance().GetComponent<Camera>(), out offsetX, out offsetY);
					screenMaskGO.transform.localPosition = new Vector3(offsetX, offsetY, screenZ) + offset;
				}
				if (screenMaskGO2 != null && camera2 != null && !fullScreen2)
				{
					float offsetX2;
					float offsetY2;
					UpdateDynamicElement(camera2, out offsetX2, out offsetY2);
					screenMaskGO2.transform.localPosition = new Vector3(offsetX2, offsetY2, screenZ) + offset;
				}
				if (uiElement == null || uiElement.gameObject == null || !uiElement.IsActive())
				{
					if (base.ParentAction.Status != SessionActionTracker.StatusCode.FINISHED_SUCCESS && base.ParentAction.Status != SessionActionTracker.StatusCode.OBLITERATED)
					{
						base.ParentAction.MarkFailed();
					}
					Destroy();
					spawnReturnCode = SessionActionManager.SpawnReturnCode.KILL;
				}
			}
		}
		else
		{
			if (maskType == ScreenMaskType.SIMULATED && (simulated == null || !simulated.Visible))
			{
				base.ParentAction.MarkFailed();
			}
			float offsetX3;
			float offsetY3;
			UpdateDynamicElement(GUIMainView.GetInstance().GetComponent<Camera>(), out offsetX3, out offsetY3);
			screenMaskGO.transform.localPosition = new Vector3(offsetX3, offsetY3, screenZ);
			spawnReturnCode = base.OnUpdate(game);
		}
		return spawnReturnCode;
	}

	public override void Destroy()
	{
		if (screenMaskGO != null)
		{
			UnityEngine.Object.Destroy(screenMaskGO);
			screenMaskGO = null;
		}
		if (screenMaskGO2 != null)
		{
			UnityEngine.Object.Destroy(screenMaskGO2);
			screenMaskGO2 = null;
			camera2 = null;
		}
		if (uiMixin != null)
		{
			uiMixin.Destroy();
			uiMixin = null;
		}
		if (simulated != null)
		{
			simulated.RemoveClickListener(simHandler);
		}
		if (slot != null)
		{
			slot.RemoveClickListener(slotHandler);
		}
	}

	private void CreateScreenMaskMesh(float radius, string texture, Vector3 offset, Camera secondCam, bool coverFullScreen = false)
	{
		Camera camera = null;
		Camera camera2 = GUIMainView.GetInstance().GetComponent<Camera>();
		camera = ((!(secondCam != null)) ? camera2 : secondCam);
		GameObject gameObject = UnityGameResources.Create("Prefabs/ScreenMask");
		gameObject.name = "ScreenMaskSpawn";
		gameObject.layer = LayerMask.NameToLayer("__GUI__");
		if (secondCam == null)
		{
			screenMaskGO = gameObject;
		}
		else
		{
			screenMaskGO2 = gameObject;
		}
		if (texture != null && texture.Length > 0)
		{
			Texture2D texture2D = Resources.Load("Textures/GUI/" + texture) as Texture2D;
			if (texture2D != null)
			{
				gameObject.GetComponent<Renderer>().material.mainTexture = texture2D;
			}
		}
		this.offset = offset;
		if (maskType == ScreenMaskType.ELEMENT)
		{
			offsetAbsMax = Math.Abs(Math.Max(offset.x, offset.y));
		}
		else
		{
			offsetAbsMax = 0f;
		}
		float num = Math.Max(offsetAbsMax, 1f);
		borderStepY = camera.orthographicSize * 2f + num;
		if (!double.IsInfinity(camera.aspect) && !double.IsNaN(camera.aspect))
		{
			borderStepX = borderStepY * camera.aspect;
		}
		else
		{
			borderStepX = borderStepY * 2f;
		}
		centerStepY = radius * 2f;
		centerStepX = centerStepY;
		float num2 = borderStepX + centerStepX * 0.5f;
		float num3 = borderStepY + centerStepY * 0.5f;
		gameObject.transform.parent = camera.transform;
		float offsetX = 0f;
		float offsetY = 0f;
		if (!coverFullScreen)
		{
			UpdateDynamicElement(camera, out offsetX, out offsetY);
		}
		else
		{
			offsetX = (borderStepX + centerStepX) * 0.5f;
			offsetY = (borderStepY + centerStepY) * 0.5f;
		}
		Vector3 localPosition = new Vector3(offsetX, offsetY, screenZ);
		if (maskType == ScreenMaskType.ELEMENT && !coverFullScreen)
		{
			localPosition += offset;
		}
		gameObject.transform.localPosition = localPosition;
		gameObject.transform.localRotation = Quaternion.identity;
		Mesh mesh = new Mesh();
		gameObject.GetComponent<MeshFilter>().mesh = mesh;
		Vector3[] array = new Vector3[16];
		Vector2[] array2 = new Vector2[16];
		int[] array3 = new int[54];
		int num4 = 0;
		float num5 = 0f - num3;
		for (int i = 0; i < 4; i++)
		{
			float num6 = 0f - num2;
			for (int j = 0; j < 4; j++)
			{
				array[num4].x = num6;
				array[num4].y = num5;
				array[num4].z = 0f;
				num6 = ((j == 1) ? (num6 + centerStepX) : (num6 + borderStepX));
				if (j <= 1)
				{
					array2[num4].x = 0f;
				}
				else
				{
					array2[num4].x = 1f;
				}
				if (i <= 1)
				{
					array2[num4].y = 0f;
				}
				else
				{
					array2[num4].y = 1f;
				}
				num4++;
			}
			num5 = ((i == 1) ? (num5 + centerStepY) : (num5 + borderStepY));
		}
		int num7 = 0;
		for (int k = 0; k < 3; k++)
		{
			int num8 = 4 * k;
			for (int l = 0; l < 3; l++)
			{
				array3[num7++] = num8;
				array3[num7++] = num8 + 4;
				array3[num7++] = num8 + 1;
				array3[num7++] = num8 + 1;
				array3[num7++] = num8 + 4;
				array3[num7++] = num8 + 4 + 1;
				num8++;
			}
		}
		TFUtils.Assert(num7 == 54, "Error in screen mesh");
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.triangles = array3;
	}

	private void UpdateDynamicElement(Camera cam, out float offsetX, out float offsetY)
	{
		float num;
		float num2;
		if (maskType == ScreenMaskType.ELEMENT)
		{
			Vector2 vector = cam.WorldToViewportPoint(uiElement.tform.position);
			num = vector.x * cam.pixelWidth;
			num2 = vector.y * cam.pixelHeight;
		}
		else if (maskType == ScreenMaskType.SIMULATED)
		{
			Vector3 vector2 = simulation.TheCamera.WorldToScreenPoint(simulated.DisplayController.Position + offset);
			num = vector2.x;
			num2 = vector2.y;
		}
		else if (maskType == ScreenMaskType.EXPANSION)
		{
			Vector3 vector3 = simulation.TheCamera.WorldToScreenPoint(slot.Position + offset);
			num = vector3.x;
			num2 = vector3.y;
		}
		else
		{
			Vector3 vector4 = simulation.TheCamera.WorldToScreenPoint(offset);
			num = vector4.x;
			num2 = vector4.y;
		}
		float m = cam.projectionMatrix.m00;
		float m2 = cam.projectionMatrix.m11;
		offsetX = 2f * (num / cam.pixelWidth - 0.5f) / m;
		offsetY = 2f * (num2 / cam.pixelHeight - 0.5f) / m2;
		ClampOffset(cam, ref offsetX, ref offsetY);
	}

	private void ClampOffset(Camera cam, ref float offsetX, ref float offsetY)
	{
		float num = cam.orthographicSize * 2f;
		float num2 = cam.orthographicSize * 2f * cam.aspect;
		float num3 = 0.5f * (num2 + centerStepX) + offsetAbsMax;
		float num4 = 0.5f * (num + centerStepY) + offsetAbsMax;
		if (offsetX < 0f - num3)
		{
			offsetX = 0f - num3;
		}
		else if (offsetX > num3)
		{
			offsetX = num3;
		}
		if (offsetY < 0f - num4)
		{
			offsetY = 0f - num4;
		}
		else if (offsetY > num4)
		{
			offsetY = num4;
		}
	}
}
