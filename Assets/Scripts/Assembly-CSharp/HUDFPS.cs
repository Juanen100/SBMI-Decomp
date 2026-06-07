using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class HUDFPS : MonoBehaviour
{
	public Rect startRect;

	public bool updateColor;

	public bool allowDrag;

	public float frequency;

	public int nbDecimal;

	private float accum;

	private int frames;

	private Color color;

	private string sFPS;

	private GUIStyle style;

	private void Start()
	{
	}

	private void Update()
	{
	}

	[DebuggerHidden]
	private IEnumerator FPS()
	{
		return null;
	}

	private void OnGUI()
	{
	}

	private void DoMyWindow(int windowID)
	{
	}
}
