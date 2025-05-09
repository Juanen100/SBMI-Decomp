using System;
using UnityEngine;

public class SBGUIActivityIndicator : SBGUIElement
{
	public float piPerSecond;

	private SBGUIAtlasImage icon;

	private SBGUILabel text;

	private Vector3 iconCenter;

	private bool running;

	private float degPerSecond;

	public Vector3 Center
	{
		set
		{
			icon.transform.localPosition = value;
			iconCenter = icon.transform.position;
		}
	}

	public void InitActivityIndicator()
	{
		icon = (SBGUIAtlasImage)FindChild("icon");
		text = (SBGUILabel)FindChild("text");
		degPerSecond = (0f - piPerSecond) * 180f / (float)Math.PI;
		iconCenter = icon.transform.position;
	}

	public void StartActivityIndicator()
	{
		if (!running)
		{
			icon.SetVisible(true);
			text.SetVisible(true);
			running = true;
		}
	}

	public void StopActivityIndicator()
	{
		if (running)
		{
			icon.SetVisible(false);
			text.SetVisible(false);
			running = false;
		}
	}

	public void Update()
	{
		if (running)
		{
			icon.transform.RotateAround(iconCenter, icon.transform.forward, degPerSecond * Time.deltaTime);
		}
	}
}
