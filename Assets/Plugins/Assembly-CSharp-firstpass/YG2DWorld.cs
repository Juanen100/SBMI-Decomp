using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using UnityEngine;

public class YG2DWorld : MonoBehaviour
{
	public Camera renderCamera;

	private readonly Vector2 GUIGravity;

	protected World world;

	public bool drawCursor;

	public bool shape;

	public bool joint;

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
			return null;
		}
	}

	public World World
	{
		get
		{
			return null;
		}
	}

	public Vector2 Cursor2D(Vector3 cursor3d)
	{
		return default(Vector2);
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	public List<Fixture> GetHitFixtures(Vector2 pos)
	{
		return null;
	}

	public static void UpdateTransform(Transform t, Body body)
	{
	}
}
