using UnityEngine;

internal class Callable : AndroidJavaProxy
{
	private SubmitAdIdCallable redirectedDelegate;

	public Callable(SubmitAdIdCallable callback)
		: base((string)null)
	{
	}

	private string call()
	{
		return null;
	}
}
