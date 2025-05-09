using UnityEngine;

public class RenderTextureManager
{
	public const int RENDERTEXTURE_CAMERA_LAYER = 21;

	public const int RENDERTEXTURE_GAMEOBJECT_STAGING_LAYER = 22;

	public const float CAM_DISTANCE_TO_SUBJECT = 7f;

	public const int RENDERTEXTURE_SQUARE_SIZE = 256;

	public static Vector3 SUBJECT_POSITION = new Vector3(0f, 0f, 0f);

	public static Vector3 RENDERTEXTURE_RIGCAM_POSITION = new Vector3(0f, 0f, 7f);

	private ULRenderTextureBatch renderTextureBatch;

	private int entryCount;

	public RenderTextureManager()
	{
		Camera.main.cullingMask &= -6291457;
		renderTextureBatch = new ULRenderTextureBatch(21);
		Camera rigCamera = renderTextureBatch.CameraRig.RigCamera;
		rigCamera.transform.position = RENDERTEXTURE_RIGCAM_POSITION;
		rigCamera.transform.LookAt(SUBJECT_POSITION);
	}

	public ULRenderTextureBatchEntry AddGameObject(GameObject gameObject, ULRenderTextureCameraRig.RelativeCamDelegate theCamDelegate, string shaderIdentifier)
	{
		ULRenderTextureCameraRig.SetRenderLayer(gameObject, 22);
		ULRenderTexture target = new ULRenderTexture(256, string.Format("RenderTexture{0}", entryCount++), shaderIdentifier);
		return renderTextureBatch.AddEntry(gameObject, target, theCamDelegate);
	}

	public void RenderEntry(ULRenderTextureBatchEntry entry)
	{
		renderTextureBatch.CameraRig.RenderSubjectToTexture(entry.subject, entry.target, entry.camDelegate);
	}
}
