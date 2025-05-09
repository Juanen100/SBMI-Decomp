using UnityEngine;

public class FingerGesturesInitializer : MonoBehaviour
{
	public FingerGestures editorGestures;

	public FingerGestures desktopGestures;

	public FingerGestures iosGestures;

	public FingerGestures androidGestures;

	public bool makePersistent = true;

	private void Awake()
	{
		if (!FingerGestures.Instance)
		{
			FingerGestures fingerGestures = ((!Application.isEditor) ? androidGestures : editorGestures);
			Debug.Log("Creating FingerGestures using " + fingerGestures.name);
			FingerGestures fingerGestures2 = Object.Instantiate(fingerGestures) as FingerGestures;
			fingerGestures2.name = fingerGestures.name;
			if (makePersistent)
			{
				Object.DontDestroyOnLoad(fingerGestures2.gameObject);
			}
		}
		Object.Destroy(base.gameObject);
	}
}
