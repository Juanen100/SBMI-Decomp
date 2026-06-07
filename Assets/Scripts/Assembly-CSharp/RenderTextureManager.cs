using UnityEngine;

public class RenderTextureManager
{
	public const int RENDERTEXTURE_CAMERA_LAYER = 21;

	public const int RENDERTEXTURE_GAMEOBJECT_STAGING_LAYER = 22;

	public const float CAM_DISTANCE_TO_SUBJECT = 7f;

	public static Vector3 SUBJECT_POSITION;

	public static Vector3 RENDERTEXTURE_RIGCAM_POSITION;

	public const int RENDERTEXTURE_SQUARE_SIZE = 256;

	private ULRenderTextureBatch renderTextureBatch;

	private int entryCount;

	public ULRenderTextureBatchEntry AddGameObject(GameObject gameObject, ULRenderTextureCameraRig.RelativeCamDelegate theCamDelegate, string shaderIdentifier)
	{
		return null;
	}

	public void RenderEntry(ULRenderTextureBatchEntry entry)
	{
	}
}
