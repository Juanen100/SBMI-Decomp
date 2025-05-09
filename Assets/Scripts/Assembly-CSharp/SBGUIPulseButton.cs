using System;
using UnityEngine;

public class SBGUIPulseButton : SBGUIAtlasButton, IPulsable
{
	public Vector2 RestingSize;

	public float Amplitude;

	public float Period;

	private DeferredPulser pulser;

	public DeferredPulser Pulser
	{
		get
		{
			return pulser;
		}
	}

	private SBGUIPulseButton()
	{
	}

	protected override void Awake()
	{
		base.Awake();
		InitializePulser(RestingSize, Amplitude, Period);
		Action value = delegate
		{
			pulser.PulseOneShot();
		};
		base.gameObject.GetComponent<TapButton>().BeginEvent.AddListener(value);
	}

	public static SBGUIPulseButton Create(SBGUIElement parent, string asset, Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		GameObject gameObject = new GameObject(string.Format("SBGUIPulseButton_{0}", SBGUIElement.InstanceID));
		SBGUIPulseButton sBGUIPulseButton = gameObject.AddComponent<SBGUIPulseButton>();
		sBGUIPulseButton.Initialize(parent, new Rect(0f, 0f, -1f, -1f), asset);
		sBGUIPulseButton.sprite.SetSize(restingSize);
		sBGUIPulseButton.InitializePulser(restingSize, amplitude, period, OnCompleteCallback);
		return sBGUIPulseButton;
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period)
	{
		InitializePulser(restingSize, amplitude, period, null);
	}

	public void InitializePulser(Vector2 restingSize, float amplitude, float period, Action OnCompleteCallback)
	{
		if (pulser != null)
		{
			pulser.Destroy();
		}
		pulser = new DeferredPulser(restingSize, amplitude, period, OnPulserUpdate, OnCompleteCallback);
	}

	public override void OnDestroy()
	{
		if (pulser != null)
		{
			pulser.Destroy();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void OnPulserUpdate()
	{
		base.sprite.SetSize(pulser.Size);
	}
}
