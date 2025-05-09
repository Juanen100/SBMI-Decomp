using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class YG2DWorld : MonoBehaviour
{
	public Camera renderCamera;

	private readonly Vector2 GUIGravity = Vector2.zero;

	protected World world;

	public bool drawCursor = true;

	public bool shape = true;

	public bool joint = true;

	public bool aabb;

	public bool pair;

	public bool centerOfMass;

	public bool debugPanel;

	public bool contactPoints;

	public bool contactNormals;

	public bool polygonPoints;

	public bool performanceGraph;

	public bool controllers;

	public Camera RenderCamera
	{
		get
		{
			if (!renderCamera)
			{
				renderCamera = base.gameObject.GetComponent<Camera>();
			}
			return renderCamera;
		}
	}

	public World World
	{
		get
		{
			if (world == null)
			{
				world = new World(GUIGravity);
			}
			return world;
		}
	}

	public Vector2 Cursor2D(Vector3 cursor3d)
	{
		if (!RenderCamera)
		{
			Debug.LogWarning("no camera attach to Yarg2DWorld");
			return Vector2.zero;
		}
		cursor3d.z = 0f - RenderCamera.transform.position.z;
		return RenderCamera.ScreenToWorldPoint(cursor3d);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public List<Fixture> GetHitFixtures(Vector2 pos)
	{
		return World.TestPointAll(Cursor2D(pos));
	}

	public static void UpdateTransform(Transform t, Body body)
	{
		Vector2 position = body.Position;
		float z = body.Rotation * 57.29578f;
		Vector3 position2 = t.position;
		position2.Set(position.x, position.y, position2.z);
		t.position = position2;
		Quaternion rotation = t.rotation;
		Vector3 eulerAngles = rotation.eulerAngles;
		eulerAngles.z = z;
		rotation.eulerAngles = eulerAngles;
		t.rotation = rotation;
	}
}
