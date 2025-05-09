using System;
using UnityEngine;

public class SBGUIPulseImage : SBGUIAtlasImage, IPulsable
{
	private DeferredPulser pulser;

	public DeferredPulser Pulser
	{
		get
		{
			return pulser;
		}
	}

	private SBGUIPulseImage()
	{
	}

	public static SBGUIPulseImage Create(SBGUIElement parent, string asset, Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIPulseImage_{0}", SBGUIElement.InstanceID));
		SBGUIPulseImage sBGUIPulseImage = gameObject.AddComponent<SBGUIPulseImage>();
		sBGUIPulseImage.Initialize(parent, new Rect(0f, 0f, -1f, -1f), asset);
		sBGUIPulseImage.sprite.SetSize(restingSize);
		sBGUIPulseImage.InitializePulser(restingSize, amplitude, period, OnCompleteCallback);
		return sBGUIPulseImage;
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period)
	{
		InitializePulser(restingSize, amplitude, period, null);
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		pulser = new DeferredPulser(restingSize, amplitude, period, OnPulserUpdate, OnCompleteCallback);
	}

	public void Destroy()
	{
		pulser.Destroy();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnPulserUpdate()
	{
		if (this != null && base.sprite != null)
		{
			base.sprite.SetSize(pulser.Size);
		}
	}
}
