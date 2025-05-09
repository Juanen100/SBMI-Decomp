#define ASSERTS_ON
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Yarg;

public class SBCamera
{
	public class AtRest : StateBehavior
	{
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
			YGEvent.TYPE type = evt.type;
			if (type == YGEvent.TYPE.TOUCH_BEGIN || type == YGEvent.TYPE.PINCH)
			{
				camera.deferredGuiEvent = evt;
				camera.ChangeState(State.Dragging);
			}
		}
	}

	public class AutoPanning : StateBehavior
	{
		private const float SPEED = 5f;

		public override void OnEnter(SBCamera camera)
		{
		}

		public override void OnLeave(SBCamera camera)
		{
			camera.autoPanTargetCameraPosition = null;
		}

		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}

		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
			if (!camera.autoPanTargetCameraPosition.HasValue)
			{
				Ray ray = camera.ScreenPointToRay(camera.ScreenCenter);
				Vector3 point;
				session.TheGame.terrain.ComputeIntersection(ray, out point);
				Vector3 vector = new Vector3(camera.autoPanTargetLookAt.x, camera.autoPanTargetLookAt.y, 0f) - point;
				camera.autoPanTargetCameraPosition = camera.targetPosition + vector;
				camera.safeDistanceWorldSqrd = camera.ScreenSpaceToTerrainSpace(new Vector2(camera.safeZonePixels, 0f), session.TheGame.terrain).sqrMagnitude;
			}
			Vector3 delta;
			if (!IsCloseEnough(camera, out delta))
			{
				delta.Normalize();
				delta *= 5f;
				camera.targetPosition += delta;
				camera.UpdateTransform(session);
			}
			else
			{
				camera.ChangeState(State.AtRest);
			}
		}

		private bool IsCloseEnough(SBCamera camera, out Vector3 delta)
		{
			delta = camera.autoPanTargetCameraPosition.Value - camera.targetPosition;
			float sqrMagnitude = delta.sqrMagnitude;
			if (sqrMagnitude <= camera.safeDistanceWorldSqrd)
			{
				return true;
			}
			return false;
		}
	}

	public class Dragging : StateBehavior
	{
		protected ZoomConstrainedMixin zoomConstraints = new ZoomConstrainedMixin();

		private PanConstrainedMixin panConstraints = new PanConstrainedMixin();

		public override void OnEnter(SBCamera camera)
		{
			InitializeDragParams(camera);
		}

		public override void OnLeave(SBCamera camera)
		{
		}

		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
			Vector2 position = evt.position;
			switch (evt.type)
			{
			case YGEvent.TYPE.TOUCH_BEGIN:
				break;
			case YGEvent.TYPE.TOUCH_MOVE:
				if (!camera.previousTouchDragCenter.HasValue)
				{
					camera.previousTouchDragCenter = position;
					camera.touchDragVectorScreen = Vector2.zero;
				}
				else if (!camera.dragNeedsUpdate)
				{
					camera.touchDragVectorScreen = camera.previousTouchDragCenter.Value - position;
					camera.previousTouchDragCenter = position;
					camera.dragNeedsUpdate = true;
				}
				break;
			case YGEvent.TYPE.PINCH:
				camera.deferredGuiEvent = evt;
				camera.ChangeState(State.ZoomDragging);
				break;
			case YGEvent.TYPE.TOUCH_END:
				camera.ChangeState(State.Stopping);
				break;
			case YGEvent.TYPE.TOUCH_CANCEL:
				camera.ChangeState(State.Stopping);
				break;
			case YGEvent.TYPE.TOUCH_STAY:
			case YGEvent.TYPE.HOVER:
			case YGEvent.TYPE.DRAG:
			case YGEvent.TYPE.FLICK:
			case YGEvent.TYPE.SWIPE:
				break;
			}
		}

		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
			if (this == null)
			{
				return;
			}
			if (!camera.allowUserInput)
			{
				camera.ChangeState(State.Stopping);
			}
			else if (camera.isDraggingBuilding)
			{
				bool flag = false;
				Vector3 zero = Vector3.zero;
				if (Input.mousePosition.x < (float)camera.xMoveScreenNumber)
				{
					zero += camera.ScreenSpaceToTerrainSpace(camera.moveCamLeft, session.TheGame.terrain);
					flag = true;
				}
				else if (Input.mousePosition.x > (float)(Screen.width - camera.xMoveScreenNumber))
				{
					zero += camera.ScreenSpaceToTerrainSpace(camera.moveCamRight, session.TheGame.terrain);
					flag = true;
				}
				if (Input.mousePosition.y > (float)(Screen.height - camera.yMoveScreenNumber))
				{
					zero += camera.ScreenSpaceToTerrainSpace(camera.moveCamDown, session.TheGame.terrain);
					flag = true;
				}
				else if (Input.mousePosition.y < (float)camera.yMoveScreenNumber)
				{
					zero += camera.ScreenSpaceToTerrainSpace(camera.moveCamUp, session.TheGame.terrain);
					flag = true;
				}
				if (flag)
				{
					camera.targetPosition = camera.previousDragPosition + zero;
					camera.previousDragPosition = camera.targetPosition;
					camera.touchDragVectorScreen = Vector2.zero;
					camera.UpdateTransform(session);
					if (panConstraints.HardKeepInBounds(session.TheGame.terrain, camera, camera.ScreenPointToWorldPoint(session.TheGame.terrain, camera.ScreenCenter)))
					{
						camera.UpdateTransform(session);
					}
					camera.dragNeedsUpdate = false;
				}
			}
			else if (camera.dragNeedsUpdate)
			{
				Vector3 vector = camera.ScreenSpaceToTerrainSpace(camera.touchDragVectorScreen, session.TheGame.terrain);
				camera.targetPosition = camera.previousDragPosition + vector;
				camera.previousDragPosition = camera.targetPosition;
				camera.touchDragVectorScreen = Vector2.zero;
				camera.UpdateTransform(session);
				if (panConstraints.HardKeepInBounds(session.TheGame.terrain, camera, camera.ScreenPointToWorldPoint(session.TheGame.terrain, camera.ScreenCenter)))
				{
					camera.UpdateTransform(session);
				}
				camera.dragNeedsUpdate = false;
			}
		}

		public override void OnResetState(SBCamera camera)
		{
			InitializeDragParams(camera);
		}

		protected void InitializeDragParams(SBCamera camera)
		{
			camera.previousDragPosition = camera.UnityCamera.transform.position;
			camera.previousTouchDragCenter = null;
			camera.touchDragVectorScreen = Vector2.zero;
			camera.dragNeedsUpdate = false;
			camera.momentum.Reset();
			camera.momentum.ClearTrackPositions();
			camera.moveCamLeft = new Vector2(-880f / camera.targetZoom, 0f);
			camera.moveCamRight = new Vector2(880f / camera.targetZoom, 0f);
			camera.moveCamUp = new Vector2(0f, -600f / camera.targetZoom);
			camera.moveCamDown = new Vector2(0f, 600f / camera.targetZoom);
		}
	}

	public class FrictionMixin
	{
		private const float GLIDE_TOLERANCE_SQARED = 0.01f;

		private const float FRICTION_FACTOR = 0.85f;

		public bool Apply(float dT, SBCamera camera)
		{
			float p = dT / EXPECTED_UPDATE_PERIOD;
			float amount = Mathf.Pow(0.85f, p);
			camera.momentum.ApplyFriction(amount);
			if (camera.momentum.Velocity.sqrMagnitude < 0.01f)
			{
				camera.momentum.Reset();
				return false;
			}
			return true;
		}
	}

	public class PanConstrainedMixin
	{
		public bool HardKeepInBounds(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			if (terrain == null || terrain.CameraExtents == null || camera.freeCameraMode)
			{
				return false;
			}
			Vector3 targetPosition = camera.targetPosition;
			float num = targetPosition.x - terrainCameraFocus.x;
			float num2 = targetPosition.y - terrainCameraFocus.y;
			AlignedBox cameraExtents = terrain.CameraExtents;
			cameraExtents = new AlignedBox(cameraExtents.xmin - 50f + num, cameraExtents.xmax + 50f + num, cameraExtents.ymin - 50f + num2, cameraExtents.ymax + 50f + num2);
			camera.targetPosition = new Vector3(Mathf.Clamp(camera.targetPosition.x, cameraExtents.xmin, cameraExtents.xmax), Mathf.Clamp(camera.targetPosition.y, cameraExtents.ymin, cameraExtents.ymax), camera.targetPosition.z);
			return camera.targetPosition != targetPosition;
		}

		public bool SmoothKeepInRestBounds(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			if (terrain != null && terrain.CameraExtents != null && !terrain.CameraExtents.Contains(terrainCameraFocus.x, terrainCameraFocus.y) && !camera.freeCameraMode)
			{
				Vector3 zero = Vector3.zero;
				if (terrainCameraFocus.x > terrain.CameraExtents.xmax)
				{
					zero.x = (terrain.CameraExtents.xmax - terrainCameraFocus.x) * 0.1f;
				}
				else if (terrainCameraFocus.x < terrain.CameraExtents.xmin)
				{
					zero.x = (terrain.CameraExtents.xmin - terrainCameraFocus.x) * 0.1f;
				}
				if (terrainCameraFocus.y > terrain.CameraExtents.ymax)
				{
					zero.y = (terrain.CameraExtents.ymax - terrainCameraFocus.y) * 0.1f;
				}
				else if (terrainCameraFocus.y < terrain.CameraExtents.ymin)
				{
					zero.y = (terrain.CameraExtents.ymin - terrainCameraFocus.y) * 0.1f;
				}
				zero.x = Mathf.Round(zero.x * 10f) / 10f;
				zero.y = Mathf.Round(zero.y * 10f) / 10f;
				zero.z = Mathf.Round(zero.z * 10f) / 10f;
				if (zero == Vector3.zero)
				{
					return false;
				}
				camera.targetPosition += zero;
				return true;
			}
			return false;
		}

		public bool SmoothKeepInteractionStrip(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			if (terrain != null && terrain.CameraExtents != null && !terrain.CameraExtents.Contains(terrainCameraFocus.x, terrainCameraFocus.y) && !camera.freeCameraMode)
			{
				if (terrainCameraFocus.x > terrain.CameraExtents.xmax)
				{
					return false;
				}
				if (terrainCameraFocus.x < terrain.CameraExtents.xmin)
				{
					return false;
				}
				if (terrainCameraFocus.y > terrain.CameraExtents.ymax)
				{
					return false;
				}
				if (terrainCameraFocus.y < terrain.CameraExtents.ymin)
				{
					return false;
				}
			}
			Vector3 zero = Vector3.zero;
			bool result = false;
			Vector2 vector = camera.WorldPointToScreenPoint(camera.interactionStripPosition3D);
			if (vector.x < (float)(Screen.width / 5))
			{
				zero = new Vector3(2f, -2f);
				camera.targetPosition += zero;
				result = true;
			}
			else if (vector.x > (float)(Screen.width - Screen.width / 5))
			{
				zero = new Vector3(-2f, 2f);
				camera.targetPosition += zero;
				result = true;
			}
			if (vector.y > (float)(Screen.height / 8 * 5))
			{
				zero = new Vector3(-4f, -4f);
				camera.targetPosition += zero;
				return true;
			}
			return result;
		}
	}

	public class Paused : StateBehavior
	{
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}
	}

	public abstract class StateBehavior
	{
		public virtual void OnEnter(SBCamera camera)
		{
		}

		public virtual void OnLeave(SBCamera camera)
		{
		}

		public virtual void OnUpdate(float dT, Session session, SBCamera camera)
		{
		}

		public virtual void OnResetState(SBCamera camera)
		{
		}

		public virtual void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}
	}

	public enum State
	{
		Paused = 0,
		AtRest = 1,
		Stopping = 2,
		Dragging = 3,
		ZoomDragging = 4,
		AutoPanning = 5
	}

	public class Stopping : AtRest
	{
		private const float SMOOTH_FACTOR = 9f;

		private FrictionMixin friction = new FrictionMixin();

		private ZoomConstrainedMixin zoomConstraints = new ZoomConstrainedMixin();

		private PanConstrainedMixin panConstraints = new PanConstrainedMixin();

		public override void OnEnter(SBCamera camera)
		{
			camera.momentum.CalculateSmoothVelocity();
		}

		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
			if (session.TheGame != null)
			{
				camera.targetPosition += camera.momentum.Velocity * dT / EXPECTED_UPDATE_PERIOD;
				camera.UpdateTransform(session);
				Terrain terrain = session.TheGame.terrain;
				Vector3 terrainCameraFocus = camera.ScreenPointToWorldPoint(terrain, camera.ScreenCenter);
				bool flag = camera.momentum.Velocity.sqrMagnitude > 0f;
				flag |= friction.Apply(dT, camera);
				flag |= zoomConstraints.SmoothKeepInRestBounds(camera);
				bool flag2 = panConstraints.HardKeepInBounds(terrain, camera, terrainCameraFocus);
				flag = flag || flag2;
				flag |= panConstraints.SmoothKeepInRestBounds(terrain, camera, terrainCameraFocus);
				if (camera.isDraggingBuilding && !flag)
				{
					flag |= panConstraints.SmoothKeepInteractionStrip(terrain, camera, terrainCameraFocus);
				}
				if (flag2)
				{
					camera.momentum.Reset();
				}
				if (!flag)
				{
					camera.momentum.Reset();
					camera.ChangeState(State.AtRest);
				}
				else
				{
					camera.UpdateTransform(session);
				}
				base.OnUpdate(dT, session, camera);
			}
		}
	}

	public class ZoomConstrainedMixin
	{
		private const float ZOOM_FRICTION_FACTOR = 0.25f;

		private const float ZOOM_TOLERANCE = 0.01f;

		private static float REST_MIN_ORTHO_SIZE;

		private static float REST_MAX_ORTHO_SIZE;

		private static float HARD_MIN_ORTHO_SIZE;

		private static float HARD_MAX_ORTHO_SIZE;

		public ZoomConstrainedMixin()
		{
			REST_MIN_ORTHO_SIZE = 60f;
			REST_MAX_ORTHO_SIZE = 200f;
			HARD_MIN_ORTHO_SIZE = REST_MIN_ORTHO_SIZE - 10f;
			HARD_MAX_ORTHO_SIZE = REST_MAX_ORTHO_SIZE + 25f;
		}

		public bool HardKeepInBounds(SBCamera camera)
		{
			if ((camera.targetZoom > HARD_MAX_ORTHO_SIZE || camera.targetZoom < HARD_MIN_ORTHO_SIZE) && !camera.freeCameraMode)
			{
				camera.targetZoom = Mathf.Clamp(camera.targetZoom, HARD_MIN_ORTHO_SIZE, HARD_MAX_ORTHO_SIZE);
				return true;
			}
			return false;
		}

		public bool SmoothKeepInRestBounds(SBCamera camera)
		{
			if (camera.freeCameraMode)
			{
				return false;
			}
			float num = 0f;
			if (camera.targetZoom > REST_MAX_ORTHO_SIZE)
			{
				num = REST_MAX_ORTHO_SIZE - camera.targetZoom;
			}
			else if (camera.targetZoom < REST_MIN_ORTHO_SIZE)
			{
				num = REST_MIN_ORTHO_SIZE - camera.targetZoom;
			}
			num *= 0.25f;
			if (Mathf.Abs(num) < 0.01f)
			{
				num = 0f;
				camera.targetZoom = Mathf.Clamp(camera.targetZoom, REST_MIN_ORTHO_SIZE, REST_MAX_ORTHO_SIZE);
				return false;
			}
			camera.targetZoom += num;
			return true;
		}
	}

	public class ZoomDragging : Dragging
	{
		public override void OnEnter(SBCamera camera)
		{
			InitializeDragParams(camera);
			camera.initialOrthoSize = camera.camera.orthographicSize;
			camera.pinchDiff = 0f;
			camera.initialPinchMagnitude = null;
		}

		public override void OnLeave(SBCamera camera)
		{
		}

		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
			switch (evt.type)
			{
			case YGEvent.TYPE.PINCH:
			{
				Vector2 vector = (evt.position + evt.startPosition) * 0.5f;
				if (!camera.initialPinchMagnitude.HasValue)
				{
					camera.initialPinchMagnitude = evt.deltaPosition.magnitude;
				}
				camera.pinchDiff = evt.deltaPosition.magnitude - camera.initialPinchMagnitude.Value;
				if (!camera.previousTouchDragCenter.HasValue)
				{
					camera.previousTouchDragCenter = vector;
					camera.touchDragVectorScreen = Vector2.zero;
				}
				else
				{
					camera.touchDragVectorScreen = camera.previousTouchDragCenter.Value - vector;
					camera.previousTouchDragCenter = vector;
					camera.dragNeedsUpdate = true;
				}
				break;
			}
			case YGEvent.TYPE.TOUCH_MOVE:
				camera.deferredGuiEvent = evt;
				camera.ChangeState(State.Dragging);
				break;
			default:
				camera.ChangeState(State.Stopping);
				break;
			}
		}

		public override void OnUpdate(float dT, Session session, SBCamera sbCamera)
		{
			if (sbCamera.initialPinchMagnitude.HasValue)
			{
				float num = -0.2f * sbCamera.pinchDiff;
				sbCamera.targetZoom = sbCamera.initialOrthoSize + num;
			}
			zoomConstraints.HardKeepInBounds(sbCamera);
			base.OnUpdate(dT, session, sbCamera);
		}
	}

	public const bool DEBUG_LOG = false;

	public const float TAP_NUDGE_TOLERANCE = 400f;

	public const double PIXEL_TO_WORLD = 0.1302;

	public const double WORLD_TO_PIXEL = 7.680491551459292;

	private const float INIT_ORTHO_SIZE = 150f;

	public const float INIT_CAMERA_X = 820f;

	public const float INIT_CAMERA_Y = 520f;

	public const float MAX_CAMERA_DRAG_Y = 815f;

	private const float NEAR_CLIP_PLANE_ZOOM_COEF = 1.5f;

	private const float PINCH_SCALE = 0.2f;

	private Vector2 autoPanTargetLookAt;

	private Vector3? autoPanTargetCameraPosition;

	private float safeZonePixels;

	private float safeDistanceWorldSqrd;

	private Vector3 previousDragPosition;

	private Vector2 touchDragVectorScreen;

	private Vector2? previousTouchDragCenter;

	private bool dragNeedsUpdate;

	private int xMoveScreenNumber = (int)((float)Screen.width / 8f);

	private int yMoveScreenNumber = (int)((float)Screen.height / 4f);

	private Vector2 moveCamLeft;

	private Vector2 moveCamRight;

	private Vector2 moveCamUp;

	private Vector2 moveCamDown;

	public static bool EnableFullSCreenQuad = false;

	private static float EXPECTED_UPDATE_PERIOD = 1f / 60f;

	private static readonly Vector3 up = new Vector3(0.4f, 0.4f, 0.9f);

	private Camera camera = Camera.main;

	private Vector3 targetPosition;

	private Momentum momentum;

	private float targetZoom;

	private bool allowUserInput = true;

	private bool isDraggingBuilding;

	private Vector3 interactionStripPosition3D;

	public bool freeCameraMode;

	private static RenderTexture offScreenRenderTexture = null;

	private static GameObject fullScreenQuadGO = null;

	private StateBehavior state;

	private Dictionary<State, StateBehavior> states = new Dictionary<State, StateBehavior>
	{
		{
			State.Paused,
			new Paused()
		},
		{
			State.AtRest,
			new AtRest()
		},
		{
			State.Stopping,
			new Stopping()
		},
		{
			State.Dragging,
			new Dragging()
		},
		{
			State.ZoomDragging,
			new ZoomDragging()
		},
		{
			State.AutoPanning,
			new AutoPanning()
		}
	};

	private SBGUIEvent deferredGuiEvent;

	private float initialOrthoSize;

	private float pinchDiff;

	private float? initialPinchMagnitude;

	public Camera UnityCamera
	{
		get
		{
			return camera;
		}
	}

	public Vector2 ScreenCenter
	{
		get
		{
			return camera.pixelRect.center;
		}
	}

	public bool ScreenBufferOn
	{
		get
		{
			Debug.Log("ScreenBufferOn: " + camera.enabled);
			return !camera.enabled;
		}
	}

	public SBCamera()
	{
		camera.transform.position = new Vector3(820f, 520f, 150f);
		camera.orthographicSize = 150f;
		targetPosition = camera.transform.position;
		targetZoom = camera.orthographicSize;
		momentum = new Momentum();
		camera.transform.LookAt(camera.transform.position + CameraDirectionDefinition(), new Vector3(1f, 1f, 1f));
		PauseStateMachine();
	}

	public void AutoPanToPosition(Vector2 worldTarget, float screenSafeZonePercentageHeight)
	{
		autoPanTargetLookAt = worldTarget;
		safeZonePixels = (float)Screen.height * (1f - screenSafeZonePercentageHeight);
		ChangeState(State.AutoPanning);
	}

    public void SetEnableUserInput(bool isEnabled)
    {
		SetEnableUserInput(isEnabled, false, Vector3.zero);
    }

    public void SetEnableUserInput(bool isEnabled, bool isDraggingBuilding)
	{
		SetEnableUserInput(isEnabled, isDraggingBuilding, Vector3.zero);
    }

    public void SetEnableUserInput(bool isEnabled, bool isDraggingBuilding, Vector3 interactionStripPosition3D)
	{
		allowUserInput = isEnabled;
		this.isDraggingBuilding = isDraggingBuilding;
		if (interactionStripPosition3D != default(Vector3))
		{
			this.interactionStripPosition3D = interactionStripPosition3D;
		}
	}

	public static Vector3 CameraDirectionDefinition()
	{
		float num = Mathf.Sqrt(2f) * Mathf.Tan((float)Math.PI / 6f);
		return new Vector3(1f, 1f, 0f - num);
	}

	public static void BillboardDefinition(Transform t, IDisplayController idc)
	{
		t.LookAt(t.position - CameraDirectionDefinition(), up);
	}

	public static Vector3 CameraUp()
	{
		return up;
	}

	public Vector2 WorldPointToScreenPoint(Vector3 worldPosition)
	{
		Vector3 vector = camera.WorldToScreenPoint(worldPosition);
		return new Vector2(vector.x, SBGUI.GetScreenHeight() - vector.y);
	}

	public Vector3 ScreenPointToWorldPoint(Terrain terrain, Vector2 screenPoint)
	{
		if (terrain == null)
		{
			return Vector3.zero;
		}
		Ray ray = camera.ScreenPointToRay(TFUtils.TruncateVector(screenPoint));
		Vector3 point;
		bool condition = terrain.ComputeIntersection(ray, out point);
		TFUtils.Assert(condition, "Could not intersect against the ground!");
		return point;
	}

	public Ray ScreenPointToRay(Vector2 position)
	{
		return camera.ScreenPointToRay(position);
	}

	public Vector3 ScreenSpaceToTerrainSpace(Vector2 cameraVector, Terrain terrain)
	{
		float magnitude = cameraVector.magnitude;
		cameraVector.Normalize();
		Ray ray = ScreenPointToRay(Vector2.zero);
		Ray ray2 = ScreenPointToRay(cameraVector);
		Vector3 point;
		bool flag = terrain.ComputeIntersection(ray, out point);
		Vector3 point2;
		bool flag2 = terrain.ComputeIntersection(ray2, out point2);
		TFUtils.Assert(flag && flag2, "Fail on display offset computation, please check on camera or terrain data.");
		return (point2 - point) * magnitude;
	}

	private void UpdateTransform(Session session)
	{
		camera.transform.position = targetPosition;
		camera.orthographicSize = targetZoom;
		camera.nearClipPlane = (0f - camera.orthographicSize) * 1.5f;
	}

	public void ResetCameraPosition()
	{
		Debug.Log("ResetCameraPosition");
		camera.transform.position = new Vector3(820f, 520f, 150f);
	}

	public void StartCamera()
	{
		Debug.Log("StartCamera");
		ActivateStateMachine();
	}

	public void StopCamera()
	{
		Debug.Log("StopCamera");
		PauseStateMachine();
	}

	public void TurnOnScreenBuffer(float zDist)
	{
		PrepareSwitchToReducedBuffer(zDist);
		camera.enabled = false;
		camera.Render();
		fullScreenQuadGO.active = true;
		Debug.Log("TurnOnScreenBuffer: " + zDist);
		StopCamera();
	}

	public void TurnOnScreenBuffer()
	{
		TurnOnScreenBuffer(18f);
	}

	public void TurnOffScreenBuffer()
	{
		offScreenRenderTexture.DiscardContents();
		offScreenRenderTexture.Release();
		camera.enabled = true;
		camera.targetTexture = null;
		fullScreenQuadGO.active = false;
		StartCamera();
	}

	private void PrepareSwitchToReducedBuffer(float zDist)
	{
		if (offScreenRenderTexture == null)
		{
			CreateScreenRenderTexture();
		}
		if (fullScreenQuadGO == null)
		{
			CreateFullScreenQuad();
		}
		fullScreenQuadGO.transform.localPosition = new Vector3(0f, 0f, zDist);
		camera.targetTexture = offScreenRenderTexture;
	}

	private static void CreateScreenRenderTexture()
	{
		int num = 1;
		if (CommonUtils.TextureLod() == CommonUtils.LevelOfDetail.Low)
		{
			num = 2;
		}
		int width = Screen.width / num;
		int height = Screen.height / num;
		int depth = 24;
		offScreenRenderTexture = new RenderTexture(width, height, depth, RenderTextureFormat.Default, RenderTextureReadWrite.Default);
		offScreenRenderTexture.wrapMode = TextureWrapMode.Clamp;
		offScreenRenderTexture.anisoLevel = 0;
		offScreenRenderTexture.filterMode = FilterMode.Bilinear;
	}

	private static void CreateFullScreenQuad()
	{
		fullScreenQuadGO = new GameObject("FullScreenQuad");
		fullScreenQuadGO.layer = LayerMask.NameToLayer("__GUI__");
		Camera camera = GUIMainView.GetInstance().GetComponent<Camera>();
		fullScreenQuadGO.transform.parent = camera.transform;
		fullScreenQuadGO.transform.localPosition = Vector3.zero;
		fullScreenQuadGO.transform.localRotation = Quaternion.identity;
		fullScreenQuadGO.AddComponent<MeshFilter>();
		MeshFilter component = fullScreenQuadGO.GetComponent<MeshFilter>();
		fullScreenQuadGO.AddComponent<MeshRenderer>();
		MeshRenderer component2 = fullScreenQuadGO.GetComponent<MeshRenderer>();
		component2.castShadows = false;
		component2.receiveShadows = false;
		Shader shader = Shader.Find("Mobile/Unlit (Supports Lightmap)");
		Material material = new Material(shader);
		material.name = fullScreenQuadGO.name + "_Mat";
		material.mainTexture = offScreenRenderTexture;
		component2.material = material;
		Mesh mesh = (component.mesh = new Mesh());
		float orthographicSize = camera.orthographicSize;
		float num = orthographicSize * camera.aspect;
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f - num, 0f - orthographicSize, 0f),
			new Vector3(0f - num, orthographicSize, 0f),
			new Vector3(num, 0f - orthographicSize, 0f),
			new Vector3(num, orthographicSize, 0f)
		};
		mesh.vertices = vertices;
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f)
		};
		mesh.triangles = new int[6] { 0, 1, 2, 2, 1, 3 };
	}

	public void OnUpdate(float dT, Session session)
	{
		TFUtils.Assert(dT > 0f, "We have regressed in time.");
		if (this != null)
		{
			state.OnUpdate(dT, session, this);
			momentum.TrackForSmoothing(camera.transform.position);
		}
	}

	public void HandleGUIEvent(SBGUIEvent evt)
	{
		if (allowUserInput)
		{
			state.OnGuiEvent(evt, this);
		}
	}

	public void ProcessExtraGuiEvent(SBGUIEvent evt)
	{
		HandleGUIEvent(evt);
	}

	public void ResetCurrentState()
	{
		state.OnResetState(this);
	}

	protected void PauseStateMachine()
	{
		state = states[State.Paused];
	}

	protected void ActivateStateMachine()
	{
		state = states[State.AtRest];
	}

	public void ChangeState(State state)
	{
		if (this.state != states[state])
		{
			this.state.OnLeave(this);
			this.state = states[state];
			this.state.OnEnter(this);
			if (deferredGuiEvent != null && !deferredGuiEvent.used)
			{
				deferredGuiEvent.used = true;
				this.state.OnGuiEvent(deferredGuiEvent, this);
			}
			deferredGuiEvent = null;
		}
	}
}
