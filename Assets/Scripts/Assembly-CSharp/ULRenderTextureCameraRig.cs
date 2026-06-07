using UnityEngine;

public class ULRenderTextureCameraRig
{
	public delegate void RelativeCamDelegate(GameObject subject, Camera cam);

	private GameObject rig;

	private Camera camera;

	private int layer;

	public int Layer
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public Camera RigCamera
	{
		get
		{
			return null;
		}
	}

	public GameObject RigGameObject
	{
		get
		{
			return null;
		}
	}

	public ULRenderTextureCameraRig()
	{
	}

	public ULRenderTextureCameraRig(int layer)
	{
	}

	public static void SetRenderLayer(GameObject gameObject, int layer)
	{
	}

	public void RenderSubjectToTexture(GameObject subject, ULRenderTexture renderTexture, RelativeCamDelegate camDelegate)
	{
	}
}
