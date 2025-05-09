using System.Collections;
using UnityEngine;

[AddComponentMenu("Utilities/HUDFPS")]
public class HUDFPS : MonoBehaviour
{
	public Rect startRect = new Rect(10f, 10f, 275f, 100f);

	public bool updateColor = true;

	public bool allowDrag = true;

	public float frequency = 0.5f;

	public int nbDecimal = 1;

	private float accum;

	private int frames;

	private Color color = Color.white;

	private string sFPS = string.Empty;

	private GUIStyle style;

	private void Start()
	{
		StartCoroutine(FPS());
	}

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			accum += Time.deltaTime / Time.timeScale;
			frames++;
		}
	}

	private IEnumerator FPS()
	{
		while (true)
		{
			float fps = ((accum != 0f) ? ((float)frames / accum) : 0f);
			sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
			color = ((fps >= 30f) ? Color.green : ((!(fps > 10f)) ? Color.yellow : Color.red));
			accum = 0f;
			frames = 0;
			yield return new WaitForSeconds(frequency);
		}
	}

	private void OnGUI()
	{
		if (style == null)
		{
			style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.white;
			style.alignment = TextAnchor.MiddleCenter;
		}
		GUI.color = ((!updateColor) ? Color.white : color);
		startRect = GUI.Window(0, startRect, DoMyWindow, string.Empty);
	}

	private void DoMyWindow(int windowID)
	{
		GUI.Label(new Rect(0f, 0f, startRect.width, startRect.height), string.Concat(sFPS, "FPS\nLast Save to server at:", SBWebFileServer.LastSuccessfulSave, "\nMemory: ", 10f, style));
		if (allowDrag)
		{
			GUI.DragWindow(new Rect(0f, 0f, Screen.width, Screen.height));
		}
	}
}
