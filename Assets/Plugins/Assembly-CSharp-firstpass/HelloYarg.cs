using UnityEngine;

public class HelloYarg : MonoBehaviour
{
	public TapButton button;

	private void Start()
	{
		if (button != null)
		{
			button.TapEvent.AddListener(delegate
			{
				Debug.Log("Hello World");
			});
			button.TapEvent.AddListener(OnButtonTap);
		}
	}

	private void OnButtonTap()
	{
		Debug.Log("Foo and Bar");
	}
}
