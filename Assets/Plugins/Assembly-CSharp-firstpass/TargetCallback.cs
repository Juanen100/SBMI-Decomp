using UnityEngine;

internal class TargetCallback : AndroidJavaProxy
{
	private AdobeTargetCallback redirectedDelegate;

	public TargetCallback(AdobeTargetCallback callback)
		: base((string)null)
	{
	}

	private void call(string content)
	{
	}
}
