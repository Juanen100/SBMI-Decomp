using System.Collections.Generic;
using UnityEngine;

public class SBCamera
{
	public class AtRest : StateBehavior
	{
		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
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
		}

		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}

		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
		}

		private bool IsCloseEnough(SBCamera camera, out Vector3 delta)
		{
			delta = default(Vector3);
			return false;
		}
	}

	public class Dragging : StateBehavior
	{
		protected ZoomConstrainedMixin zoomConstraints;

		private PanConstrainedMixin panConstraints;

		public override void OnEnter(SBCamera camera)
		{
		}

		public override void OnLeave(SBCamera camera)
		{
		}

		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}

		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
		}

		public override void OnResetState(SBCamera camera)
		{
		}

		protected void InitializeDragParams(SBCamera camera)
		{
		}
	}

	public class FrictionMixin
	{
		private const float GLIDE_TOLERANCE_SQARED = 0.01f;

		private const float FRICTION_FACTOR = 0.85f;

		public bool Apply(float dT, SBCamera camera)
		{
			return false;
		}
	}

	public class PanConstrainedMixin
	{
		public bool HardKeepInBounds(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			return false;
		}

		public bool SmoothKeepInRestBounds(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			return false;
		}

		public bool SmoothKeepInteractionStrip(Terrain terrain, SBCamera camera, Vector3 terrainCameraFocus)
		{
			return false;
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
		private FrictionMixin friction;

		private ZoomConstrainedMixin zoomConstraints;

		private PanConstrainedMixin panConstraints;

		private const float SMOOTH_FACTOR = 9f;

		public override void OnEnter(SBCamera camera)
		{
		}

		public override void OnUpdate(float dT, Session session, SBCamera camera)
		{
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

		public bool HardKeepInBounds(SBCamera camera)
		{
			return false;
		}

		public bool SmoothKeepInRestBounds(SBCamera camera)
		{
			return false;
		}
	}

	public class ZoomDragging : Dragging
	{
		public override void OnEnter(SBCamera camera)
		{
		}

		public override void OnLeave(SBCamera camera)
		{
		}

		public override void OnGuiEvent(SBGUIEvent evt, SBCamera camera)
		{
		}

		public override void OnUpdate(float dT, Session session, SBCamera sbCamera)
		{
		}
	}

	private Vector2 autoPanTargetLookAt;

	private Vector3? autoPanTargetCameraPosition;

	private float safeZonePixels;

	private float safeDistanceWorldSqrd;

	private Vector3 previousDragPosition;

	private Vector2 touchDragVectorScreen;

	private Vector2? previousTouchDragCenter;

	private bool dragNeedsUpdate;

	private int xMoveScreenNumber;

	private int yMoveScreenNumber;

	private Vector2 moveCamLeft;

	private Vector2 moveCamRight;

	private Vector2 moveCamUp;

	private Vector2 moveCamDown;

	public const bool DEBUG_LOG = false;

	public const float TAP_NUDGE_TOLERANCE = 400f;

	public const double PIXEL_TO_WORLD = 0.1302;

	public const double WORLD_TO_PIXEL = 7.680491551459292;

	public static bool EnableFullSCreenQuad;

	private const float INIT_ORTHO_SIZE = 150f;

	public const float INIT_CAMERA_X = 820f;

	public const float INIT_CAMERA_Y = 520f;

	public const float MAX_CAMERA_DRAG_Y = 815f;

	private const float NEAR_CLIP_PLANE_ZOOM_COEF = 1.5f;

	private static float EXPECTED_UPDATE_PERIOD;

	private static readonly Vector3 up;

	private Camera camera;

	private Vector3 targetPosition;

	private Momentum momentum;

	private float targetZoom;

	private bool allowUserInput;

	private bool isDraggingBuilding;

	private Vector3 interactionStripPosition3D;

	public bool freeCameraMode;

	private static RenderTexture offScreenRenderTexture;

	private static GameObject fullScreenQuadGO;

	private StateBehavior state;

	private Dictionary<State, StateBehavior> states;

	private SBGUIEvent deferredGuiEvent;

	private const float PINCH_SCALE = 0.2f;

	private float initialOrthoSize;

	private float pinchDiff;

	private float? initialPinchMagnitude;

	public Camera UnityCamera
	{
		get
		{
			return null;
		}
	}

	public Vector2 ScreenCenter
	{
		get
		{
			return default(Vector2);
		}
	}

	public bool ScreenBufferOn
	{
		get
		{
			return false;
		}
	}

	public void AutoPanToPosition(Vector2 worldTarget, float screenSafeZonePercentageHeight)
	{
	}

	public void SetEnableUserInput(bool isEnabled, bool isDraggingBuilding = false, Vector3 interactionStripPosition3D = default(Vector3))
	{
	}

	public static Vector3 CameraDirectionDefinition()
	{
		return default(Vector3);
	}

	public static void BillboardDefinition(Transform t, IDisplayController idc)
	{
	}

	public static Vector3 CameraUp()
	{
		return default(Vector3);
	}

	public Vector2 WorldPointToScreenPoint(Vector3 worldPosition)
	{
		return default(Vector2);
	}

	public Vector3 ScreenPointToWorldPoint(Terrain terrain, Vector2 screenPoint)
	{
		return default(Vector3);
	}

	public Ray ScreenPointToRay(Vector2 position)
	{
		return default(Ray);
	}

	public Vector3 ScreenSpaceToTerrainSpace(Vector2 cameraVector, Terrain terrain)
	{
		return default(Vector3);
	}

	private void UpdateTransform(Session session)
	{
	}

	public void ResetCameraPosition()
	{
	}

	public void StartCamera()
	{
	}

	public void StopCamera()
	{
	}

	public void TurnOnScreenBuffer(float zDist)
	{
	}

	public void TurnOnScreenBuffer()
	{
	}

	public void TurnOffScreenBuffer()
	{
	}

	private void PrepareSwitchToReducedBuffer(float zDist)
	{
	}

	private static void CreateScreenRenderTexture()
	{
	}

	private static void CreateFullScreenQuad()
	{
	}

	public void OnUpdate(float dT, Session session)
	{
	}

	public void HandleGUIEvent(SBGUIEvent evt)
	{
	}

	public void ProcessExtraGuiEvent(SBGUIEvent evt)
	{
	}

	public void ResetCurrentState()
	{
	}

	protected void PauseStateMachine()
	{
	}

	protected void ActivateStateMachine()
	{
	}

	public void ChangeState(State state)
	{
	}
}
