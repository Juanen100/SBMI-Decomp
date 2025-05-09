using System;
using System.Collections.Generic;
using UnityEngine;

public class DeferredDisplayController : IDisplayController
{
	private float alpha;

	private Color color;

	private Vector3 position;

	private Vector3 scale;

	private Vector3 billboardScale;

	private int levelOfDetail;

	private string defaultDisplayState;

	private DisplayControllerFlags flags;

	private bool perspectiveInArt;

	private BillboardDelegate billboardDelegate;

	private IDisplayController idc;

	private IDisplayController source;

	private DisplayControllerManager dcm;

	public int LevelOfDetail
	{
		get
		{
			return (idc != null) ? idc.LevelOfDetail : levelOfDetail;
		}
		set
		{
			levelOfDetail = value;
			if (idc != null)
			{
				idc.LevelOfDetail = value;
			}
		}
	}

	public bool Visible
	{
		get
		{
			return idc != null && idc.Visible;
		}
		set
		{
			if (value)
			{
				InternalDisplayController.Visible = value;
			}
			else if (idc != null)
			{
				Destroy();
			}
		}
	}

	public bool IsDestroyed
	{
		get
		{
			return idc == null || idc.IsDestroyed;
		}
	}

	public float Alpha
	{
		get
		{
			return (idc != null) ? idc.Alpha : alpha;
		}
		set
		{
			alpha = value;
			if (idc != null)
			{
				idc.Alpha = value;
			}
		}
	}

	public Color Color
	{
		get
		{
			return (idc != null) ? idc.Color : color;
		}
		set
		{
			color = value;
			if (idc != null)
			{
				idc.Color = value;
			}
		}
	}

	public string DefaultDisplayState
	{
		get
		{
			return (idc != null) ? idc.DefaultDisplayState : defaultDisplayState;
		}
		set
		{
			defaultDisplayState = value;
			if (idc != null)
			{
				idc.DefaultDisplayState = value;
			}
		}
	}

	public Vector3 Position
	{
		get
		{
			return (idc != null) ? idc.Position : position;
		}
		set
		{
			position = value;
			if (idc != null)
			{
				idc.Position = value;
			}
		}
	}

	public Vector3 BillboardScaling
	{
		get
		{
			return (idc != null) ? idc.BillboardScaling : billboardScale;
		}
		set
		{
			billboardScale = value;
			if (idc != null)
			{
				idc.BillboardScaling = value;
			}
		}
	}

	public Vector3 Scale
	{
		get
		{
			return (idc != null) ? idc.Scale : scale;
		}
		set
		{
			scale = value;
			if (idc != null)
			{
				idc.Scale = value;
			}
		}
	}

	public DisplayControllerFlags Flags
	{
		get
		{
			return (idc != null) ? idc.Flags : flags;
		}
		set
		{
			flags = value;
			if (idc != null)
			{
				idc.Flags = value;
			}
		}
	}

	public bool isPerspectiveInArt
	{
		get
		{
			return (idc != null) ? idc.isPerspectiveInArt : perspectiveInArt;
		}
		set
		{
			perspectiveInArt = value;
			if (idc != null)
			{
				idc.isPerspectiveInArt = value;
			}
		}
	}

	public Vector3 Forward
	{
		get
		{
			return InternalDisplayController.Forward;
		}
	}

	public Vector3 Up
	{
		get
		{
			return InternalDisplayController.Up;
		}
	}

	public float Width
	{
		get
		{
			return InternalDisplayController.Width;
		}
	}

	public float Height
	{
		get
		{
			return InternalDisplayController.Height;
		}
	}

	public Transform Transform
	{
		get
		{
			return InternalDisplayController.Transform;
		}
	}

	public int NumberOfLevelsOfDetail
	{
		get
		{
			return InternalDisplayController.NumberOfLevelsOfDetail;
		}
	}

	public int MaxLevelOfDetail
	{
		get
		{
			return InternalDisplayController.MaxLevelOfDetail;
		}
	}

	public bool isVisible
	{
		get
		{
			return idc != null && idc.isVisible;
		}
	}

	public string MaterialName
	{
		get
		{
			return (idc != null) ? idc.MaterialName : null;
		}
	}

	public QuadHitObject HitObject
	{
		get
		{
			return InternalDisplayController.HitObject;
		}
	}

	private IDisplayController InternalDisplayController
	{
		get
		{
			if (idc != null)
			{
				return idc;
			}
			idc = source.CloneAndSetVisible(dcm);
			if (billboardDelegate != null)
			{
				idc.Billboard(billboardDelegate);
			}
			idc.Position = position;
			if (color != source.Color)
			{
				idc.Color = color;
			}
			if (alpha != source.Alpha)
			{
				idc.Alpha = alpha;
			}
			if (scale != source.Scale)
			{
				idc.Scale = scale;
			}
			if (billboardScale != source.BillboardScaling)
			{
				idc.BillboardScaling = BillboardScaling;
			}
			if (levelOfDetail != source.LevelOfDetail)
			{
				idc.LevelOfDetail = levelOfDetail;
			}
			if (flags != source.Flags)
			{
				idc.Flags = flags;
			}
			if (defaultDisplayState != source.DefaultDisplayState)
			{
				idc.DefaultDisplayState = defaultDisplayState;
			}
			if (perspectiveInArt != source.isPerspectiveInArt)
			{
				idc.isPerspectiveInArt = perspectiveInArt;
			}
			return idc;
		}
	}

	public DeferredDisplayController(IDisplayController source, DisplayControllerManager dcm)
	{
		this.dcm = dcm;
		this.source = source;
		scale = source.Scale;
		position = source.Position;
		alpha = source.Alpha;
		color = source.Color;
		flags = source.Flags;
		levelOfDetail = source.LevelOfDetail;
		billboardScale = source.BillboardScaling;
		defaultDisplayState = source.DefaultDisplayState;
		perspectiveInArt = source.isPerspectiveInArt;
	}

	public IDisplayController Clone(DisplayControllerManager dcm)
	{
		throw new NotImplementedException("You should not be cloning a deferred display controller!");
	}

	public IDisplayController CloneWithHitMesh(DisplayControllerManager dcm, string hitMeshName, bool separateTap = false)
	{
		throw new NotImplementedException("You should not be cloning a deferred display controller!");
	}

	public IDisplayController CloneAndSetVisible(DisplayControllerManager dcm)
	{
		throw new NotImplementedException("You should not be cloning a deferred display controller!");
	}

	public string GetDisplayState()
	{
		return (idc != null) ? idc.GetDisplayState() : null;
	}

	public void AddDisplayState(Dictionary<string, object> dict)
	{
		InternalDisplayController.AddDisplayState(dict);
	}

	public bool Intersects(Ray ray)
	{
		return InternalDisplayController.Intersects(ray);
	}

	public void ChangeMesh(string state, string meshName)
	{
		if (state == null)
		{
			Destroy();
		}
		else
		{
			InternalDisplayController.ChangeMesh(state, meshName);
		}
	}

	public void DisplayState(string state)
	{
		if (state == null)
		{
			Destroy();
		}
		else
		{
			InternalDisplayController.DisplayState(state);
		}
	}

	public void UpdateMaterialOrTexture(string material)
	{
		if (material == null)
		{
			Destroy();
		}
		else
		{
			InternalDisplayController.UpdateMaterialOrTexture(material);
		}
	}

	public void SetMaskPercentage(float pct)
	{
		InternalDisplayController.SetMaskPercentage(pct);
	}

	public void Billboard(BillboardDelegate billboard)
	{
		billboardDelegate = billboard;
		if (idc != null)
		{
			idc.Billboard(billboardDelegate);
		}
	}

	public void OnUpdate(Camera sceneCamera, ParticleSystemManager psm)
	{
		if (idc != null)
		{
			idc.OnUpdate(sceneCamera, psm);
		}
	}

	public void Destroy()
	{
		if (idc != null)
		{
			idc.Destroy();
			idc = null;
		}
	}

	public void AttachGUIElementToTarget(SBGUIElement element, string target)
	{
		InternalDisplayController.AttachGUIElementToTarget(element, target);
	}
}
