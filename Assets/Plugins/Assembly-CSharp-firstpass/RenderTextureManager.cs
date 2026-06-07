using System.Collections.Generic;
using UnityEngine;

public class RenderTextureManager
{
	private const int MaxAtlases = 4;

	private List<RenderTextureBuffer> mBufferList;

	private static RenderTextureManager sActive;

	private RenderTextureBuffer.QualityMode mQuality;

	public static RenderTextureManager Active
	{
		get
		{
			return null;
		}
	}

	public static void CreateActive(RenderTextureBuffer.QualityMode quality)
	{
	}

	public bool AddTexture(Texture texture, bool destroyOnLoad, bool processInstantly = false)
	{
		return false;
	}

	public void UpdateRenderBuffers()
	{
	}

	public static void DestroyActive()
	{
	}
}
