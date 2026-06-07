using UnityEngine;

internal class AudienceManagerCallback : AndroidJavaProxy
{
	private AdobeAudienceManagerCallback redirectedDelegate;

	public AudienceManagerCallback(AdobeAudienceManagerCallback callback)
		: base((string)null)
	{
	}

	private void call(AndroidJavaObject content)
	{
	}
}
